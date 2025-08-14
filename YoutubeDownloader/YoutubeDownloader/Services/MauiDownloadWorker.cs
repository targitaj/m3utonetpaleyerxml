using System.Net;
using System.Net.Http.Headers;
using VideoLibrary;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace YoutubeDownloader;

// Рабочий класс без UI. Передавайте коллбеки для статуса/прогресса по желанию.
public sealed class MauiDownloadWorker
{
    private readonly IFFmpegService _ffmpegService; // ваш сервис для Android (FFmpegKit и т.п.)

    public MauiDownloadWorker(IFFmpegService ffmpegService)
    {
        _ffmpegService = ffmpegService;
    }

    /// <summary>
    /// Скачивает видео/аудио и склеивает в один файл.
    /// </summary>
    public async Task DownloadAsync(
        string videoUrl,
        bool isMaxResolution,
        Action<double>? report = null,     // 0..1
        Action<string>? status = null,
        CancellationToken ct = default)
    {
        status?.Invoke("Init YouTube client");

        // 1) Получаем доступные потоки (как в вашей VM)
        var youTube = YouTube.Default;
        status?.Invoke("GetAllVideosAsync");
        var all = await youTube.GetAllVideosAsync(videoUrl);

        // 2) Путь к ffmpeg и директории загрузок
                var ffmpegPath = Path.Combine(GetPath(), "ffmpeg");

        //#if !ANDROID
        //        // На desktop докачиваем ffmpeg при необходимости (Android — пропускаем)
        //        if (!File.Exists(ffmpegPath))
        //        {
        //            status?.Invoke("Download ffmpeg");
        //            var prog = new Progress<Xabe.FFmpeg.Downloader.ProgressInfo>(p =>
        //            {
        //                if (p.TotalBytes > 0) report?.Invoke(Math.Clamp((double)p.DownloadedBytes / p.TotalBytes, 0, 1) * 0.1);
        //            });
        //            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full, ffmpegPath, prog);
        //        }
        //#endif

        var downloadsDir = Path.Combine(GetPath(), "youtube");
        Directory.CreateDirectory(downloadsDir);

        // 3) Выбираем видеопоток и аудио (как у вас: mp4, макс/мин по резолюции; аудио — макс битрейт) :contentReference[oaicite:2]{index=2}
        var video = isMaxResolution
            ? all.OrderByDescending(o => o.Resolution).First(f => f.Format == VideoFormat.Mp4)
            : all.OrderBy(o => o.Resolution).First(f => f.Format == VideoFormat.Mp4 && f.Resolution >= 360);

        var vidUrl = video.Uri;
        var videoPath = Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");

        //File.Create(Path.Combine(downloadsDir, $"test.txt"));

        status?.Invoke("Download video");
        await DownloadFileWithResumeAsync(vidUrl, videoPath, p => report?.Invoke(0.1 + p * 0.35), ct); // до 45%

        var audio = all.OrderByDescending(o => o.AudioBitrate).First();
        var audUrl = audio.Uri;
        var audioPath = Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");

        status?.Invoke("Download audio");
        await DownloadFileWithResumeAsync(audUrl, audioPath, p => report?.Invoke(0.45 + p * 0.35), ct); // до 80%

        // 4) Склейка (как в вашей VM: -shortest, на Android зовём ваш сервис) :contentReference[oaicite:3]{index=3}
        var outPath = Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");
        var command = $"-i \"{videoPath}\" -i \"{audioPath}\" -shortest \"{outPath}\"";

        status?.Invoke("Merging audio and video");
#if ANDROID
        await _ffmpegService.ExecuteAsync(command);  // ваш путь на Android
#else
        FFmpeg.SetExecutablesPath(ffmpegPath);
        await new Conversion().Start(command);       // как у вас
#endif

        report?.Invoke(1.0);
        status?.Invoke("Done");
    }

    // === Helpers ===

    private string GetPath()
    {
#if ANDROID
        return _ffmpegService.GetPath(); // как в вашей VM для Android :contentReference[oaicite:4]{index=4}
#else
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
#endif
    }

    // Взято из вашей VM: докачка с Range + прогресс, только без UI и с токеном отмены :contentReference[oaicite:5]{index=5} :contentReference[oaicite:6]{index=6}
    private static async Task DownloadFileWithResumeAsync(
        string downloadUrl,
        string filePath,
        Action<double>? progress,
        CancellationToken ct)
    {
        long offset = 0;
        if (File.Exists(filePath))
            offset = new FileInfo(filePath).Length;

        var request = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
        if (offset > 0)
            request.Headers.Range = new RangeHeaderValue(offset, null);

        using var http = new HttpClient();
        using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

        if (response.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
            return;

        response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength ?? 0L;
        long totalRead = offset;
        byte[] buffer = new byte[8192];

        using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, 8192, useAsync: true);

        while (true)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
            if (read == 0) break;

            await fs.WriteAsync(buffer.AsMemory(0, read), ct);
            totalRead += read;

            var denom = offset + contentLength;
            var p = denom > 0 ? (double)totalRead / denom : 0;
            progress?.Invoke(Math.Clamp(p, 0, 1));
        }
    }
}
