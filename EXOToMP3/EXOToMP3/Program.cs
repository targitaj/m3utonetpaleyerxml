using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PsshInspector
{
    enum EntryType { Unknown, Init, Segment, Error }

    sealed class Options
    {
        public string Path { get; set; } = ".";
        public int MaxBytesToScan { get; set; } = 2 * 1024 * 1024; // init обычно маленький
    }

    sealed class ExoEntry
    {
        public string Path = "";
        public string Name = "";
        public EntryType Type = EntryType.Unknown;
        public bool HasFtyp, HasMoov, HasMoof, HasMdat;
    }

    sealed class PsshInfo
    {
        public int Index;
        public int Version;          // 0 или 1
        public string SystemId = ""; // UUID
        public string SystemName = "";// Widevine/PlayReady/...
        public int KidCount;         // только при version==1
        public int DataSize;         // размер payload
    }

    sealed class TencInfo
    {
        public int Index;
        public int Version;            // 0 или 1
        public int IsEncrypted;        // 0/1
        public int PerSampleIvSize;    // 0/8/16
        public string DefaultKid = ""; // UUID
        public int ConstantIvSize;     // если version==1
        public string ConstantIvHex;   // если есть
    }

    static class Program
    {
        // Известные DRM UUID (big-endian как в MP4)
        static readonly Dictionary<string, string> DrmByUuid =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "edef8ba9-79d6-4ace-a3c8-27dcd51d21ed", "Widevine" },
                { "9a04f079-9840-4286-ab92-e65be0885f95", "PlayReady" },
                { "94ce86fb-07ff-4f43-adb8-93d2fa968ca2", "FairPlay" },
                { "e2719d58-a985-b3c9-781a-b030af78d30e", "ClearKey" },
                { "5e629af5-38da-4063-8977-97ffbd9902d4", "Marlin" },
                { "f239e769-efa3-4850-9c16-a903c6932efb", "Adobe Primetime" }
            };

        static int Main(string[] args)
        {
            var opt = ParseArgs(args);
            var root = Path.GetFullPath(opt.Path);
            if (!Directory.Exists(root))
            {
                Console.WriteLine("[ERR ] Path not found: " + root);
                return 1;
            }

            Console.WriteLine("[INFO] Scan: " + root);

            var files = Directory.EnumerateFiles(root, "*.exo", SearchOption.AllDirectories).ToList();
            if (files.Count == 0)
            {
                Console.WriteLine("[WARN] No .exo files found.");
                return 0;
            }

            // Классификация init по сигнатурам
            var inits = new List<ExoEntry>();
            foreach (var f in files)
            {
                try
                {
                    var e = Classify(f, opt.MaxBytesToScan);
                    if (e.Type == EntryType.Init) inits.Add(e);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[WARN] Error reading " + f + ": " + ex.Message);
                }
            }

            if (inits.Count == 0)
            {
                Console.WriteLine("[WARN] No init files (ftyp+moov without moof/mdat) were found.");
                return 0;
            }

            Console.WriteLine("[INFO] Init files found: " + inits.Count);
            Console.WriteLine();

            foreach (var init in inits)
            {
                Console.WriteLine("File: " + init.Path);

                byte[] data = File.ReadAllBytes(init.Path);

                // schm (cenc/cbcs)
                string schemeType; uint schemeVersion; string schemeUri;
                var hasSchm = FindSchm(data, out schemeType, out schemeVersion, out schemeUri);
                if (hasSchm)
                {
                    Console.WriteLine("  schm: type=" + schemeType + ", version=" + schemeVersion + (schemeUri != null ? ", uri=" + schemeUri : ""));
                }

                // Маркеры шифрования в init
                var encMarkers = FindEncryptionMarkers(data);
                if (encMarkers.Count > 0)
                {
                    Console.WriteLine("  encryption markers: " + string.Join(",", encMarkers));
                }

                // tenc (Track Encryption Box)
                var tencs = ParseAllTenc(data);
                if (tencs.Count > 0)
                {
                    Console.WriteLine("  tenc boxes: " + tencs.Count);
                    foreach (var t in tencs)
                    {
                        var ci = (t.Version == 1 && t.ConstantIvHex != null) ? (", constIV=" + t.ConstantIvHex) : "";
                        Console.WriteLine($"    [{t.Index}] ver={t.Version}, is_encrypted={t.IsEncrypted}, per_sample_iv_size={t.PerSampleIvSize}, default_KID={t.DefaultKid}{ci}");
                    }
                }

                // pssh
                var psshs = ParseAllPssh(data);
                if (psshs.Count == 0)
                {
                    Console.WriteLine("  pssh: (none)");
                }
                else
                {
                    Console.WriteLine("  pssh boxes: " + psshs.Count);
                    int i = 1;
                    foreach (var p in psshs)
                    {
                        Console.WriteLine($"    [{i}] systemId={p.SystemId} ({p.SystemName}), version={p.Version}, kids={p.KidCount}, data={p.DataSize} bytes");
                        i++;
                    }
                }

                Console.WriteLine();
            }

            return 0;
        }

        // --------- Парсинг и поиск ---------

        static ExoEntry Classify(string path, int maxBytes)
        {
            byte[] buf = ReadHead(path, maxBytes);
            string ascii = ToAscii(buf);

            bool ftyp = ascii.Contains("ftyp");
            bool moov = ascii.Contains("moov");
            bool moof = ascii.Contains("moof");
            bool mdat = ascii.Contains("mdat");

            return new ExoEntry
            {
                Path = path,
                Name = Path.GetFileName(path),
                HasFtyp = ftyp,
                HasMoov = moov,
                HasMoof = moof,
                HasMdat = mdat,
                Type = (ftyp && moov && !moof && !mdat) ? EntryType.Init
                      : (moof ? EntryType.Segment : EntryType.Unknown)
            };
        }

        static List<PsshInfo> ParseAllPssh(byte[] file)
        {
            var res = new List<PsshInfo>();
            int idx = 0, found = 0;
            while (idx + 8 <= file.Length)
            {
                uint size = ReadU32(file, idx);
                if (size < 8) { idx++; continue; }
                if (idx + size > file.Length) { idx++; continue; }
                string type = ReadType(file, idx + 4);
                if (type == "pssh")
                {
                    var info = ParsePsshBox(file, idx, (int)size);
                    if (info != null)
                    {
                        found++;
                        info.Index = found;
                        res.Add(info);
                    }
                }
                idx += (int)size;
            }
            return res;
        }

        static PsshInfo ParsePsshBox(byte[] b, int off, int size)
        {
            int p = off + 8;                    // пропускаем header (size+type)
            if (p + 4 > off + size) return null;
            byte version = b[p]; p += 1;        // version
            p += 3;                             // flags
            if (p + 16 > off + size) return null;

            string sysId = BytesToUuid(b, p); p += 16;

            int kidCount = 0;
            if (version > 0)
            {
                if (p + 4 > off + size) return null;
                kidCount = (int)ReadU32(b, p); p += 4;
                int kidsBytes = kidCount * 16;
                if (p + kidsBytes > off + size) return null;
                p += kidsBytes;
            }

            if (p + 4 > off + size) return null;
            int dataSize = (int)ReadU32(b, p); p += 4;
            if (p + dataSize > off + size) dataSize = Math.Max(0, off + size - p);

            string name;
            if (!DrmByUuid.TryGetValue(sysId, out name)) name = "Unknown DRM";

            return new PsshInfo
            {
                Version = version,
                SystemId = sysId,
                SystemName = name,
                KidCount = kidCount,
                DataSize = dataSize
            };
        }

        static List<TencInfo> ParseAllTenc(byte[] file)
        {
            var res = new List<TencInfo>();
            int idx = 0, found = 0;
            while (idx + 8 <= file.Length)
            {
                uint size = ReadU32(file, idx);
                if (size < 8 || idx + size > file.Length) { idx++; continue; }
                string type = ReadType(file, idx + 4);
                if (type == "tenc")
                {
                    int p = idx + 8;
                    if (p + 4 <= idx + size)
                    {
                        byte ver = file[p]; p += 1; p += 3; // version + flags
                        if (p + 1 + 1 + 16 <= idx + size)
                        {
                            int isEnc = file[p]; p += 1;
                            int perIv = file[p]; p += 1;
                            string kid = BytesToUuid(file, p); p += 16;

                            int constIvSize = 0; string constIvHex = null;
                            if (ver == 1 && p + 1 <= idx + size)
                            {
                                constIvSize = file[p]; p += 1;
                                if (constIvSize > 0 && p + constIvSize <= idx + size)
                                {
                                    constIvHex = BitConverter.ToString(file, p, constIvSize).Replace("-", "").ToLowerInvariant();
                                    p += constIvSize;
                                }
                            }

                            found++;
                            res.Add(new TencInfo
                            {
                                Index = found,
                                Version = ver,
                                IsEncrypted = isEnc,
                                PerSampleIvSize = perIv,
                                DefaultKid = kid,
                                ConstantIvSize = constIvSize,
                                ConstantIvHex = constIvHex
                            });
                        }
                    }
                }
                idx += (int)size;
            }
            return res;
        }

        static bool FindSchm(byte[] file, out string schemeType, out uint schemeVersion, out string schemeUri)
        {
            schemeType = null; schemeVersion = 0; schemeUri = null;
            int idx = 0;
            while (idx + 8 <= file.Length)
            {
                uint size = ReadU32(file, idx);
                if (size < 8) { idx++; continue; }
                if (idx + size > file.Length) break;

                string type = ReadType(file, idx + 4);
                if (type == "schm")
                {
                    int p = idx + 8;
                    if (p + 12 <= idx + size)
                    {
                        byte version = file[p]; p += 1;
                        int flags = (file[p] << 16) | (file[p + 1] << 8) | file[p + 2]; p += 3;
                        schemeType = ReadType(file, p); p += 4;
                        schemeVersion = ReadU32(file, p); p += 4;

                        if ((flags & 0x000001) != 0)
                        {
                            int uriLen = (idx + (int)size) - p;
                            if (uriLen > 0) schemeUri = SafeAscii(file, p, uriLen).TrimEnd('\0');
                        }
                        return true;
                    }
                }
                idx += (int)size;
            }
            return false;
        }

        static List<string> FindEncryptionMarkers(byte[] file)
        {
            var markers = new List<string>();
            string ascii = ToAscii(file);
            foreach (var m in new[] { "senc", "saio", "saiz", "tenc", "cenc", "cbcs" })
                if (ascii.IndexOf(m, StringComparison.Ordinal) >= 0) markers.Add(m);
            return markers;
        }

        // --------- Utils ---------

        static byte[] ReadHead(string path, int maxBytes)
        {
            var fi = new FileInfo(path);
            int n = (int)Math.Min(fi.Length, maxBytes);
            var buf = new byte[n];
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int r = fs.Read(buf, 0, n);
                if (r < n) Array.Resize(ref buf, r);
            }
            return buf;
        }

        static string ToAscii(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[i] = (b >= 32 && b <= 126) ? (char)b : ' ';
            }
            return new string(chars);
        }

        static uint ReadU32(byte[] b, int o)
        {
            return (uint)((b[o] << 24) | (b[o + 1] << 16) | (b[o + 2] << 8) | b[o + 3]);
        }

        static string ReadType(byte[] b, int o)
        {
            if (o + 4 > b.Length) return "";
            return new string(new[] { (char)b[o], (char)b[o + 1], (char)b[o + 2], (char)b[o + 3] });
        }

        static string BytesToUuid(byte[] b, int o)
        {
            string Hex(int i) => b[o + i].ToString("x2");
            return string.Concat(
                Hex(0), Hex(1), Hex(2), Hex(3), "-",
                Hex(4), Hex(5), "-",
                Hex(6), Hex(7), "-",
                Hex(8), Hex(9), "-",
                Hex(10), Hex(11), Hex(12), Hex(13), Hex(14), Hex(15)
            );
        }

        static string SafeAscii(byte[] b, int o, int len)
        {
            int n = Math.Min(len, b.Length - o);
            var chars = new char[n];
            for (int i = 0; i < n; i++)
            {
                byte v = b[o + i];
                chars[i] = (v >= 32 && v <= 126) ? (char)v : ' ';
            }
            return new string(chars);
        }

        // --------- Args ---------

        static Options ParseArgs(string[] args)
        {
            var o = new Options();
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i];
                string Next() => (i + 1 < args.Length) ? args[++i] : throw new ArgumentException("Missing value after " + a);

                switch (a.ToLowerInvariant())
                {
                    case "--path": o.Path = Next(); break;
                    case "--max-bytes": o.MaxBytesToScan = int.Parse(Next(), CultureInfo.InvariantCulture); break;
                    case "-h": case "--help": case "/?": PrintHelp(); Environment.Exit(0); break;
                    default:
                        Console.WriteLine("Unknown arg: " + a);
                        PrintHelp(); Environment.Exit(1); break;
                }
            }
            return o;
        }

        static void PrintHelp()
        {
            Console.WriteLine("PsshInspector — print DRM metadata from Exo init files (.exo)");
            Console.WriteLine("Usage: PsshInspector.exe [--path <dir>] [--max-bytes <N>]");
            Console.WriteLine("Finds init (ftyp+moov, no moof/mdat), prints pssh UUIDs (Widevine/PlayReady/etc), tenc (default_KID), scheme (cenc/cbcs). No decryption.");
        }
    }
}
