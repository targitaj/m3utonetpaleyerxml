using System.Threading.Tasks;
using Android.Runtime;
using FFMpegKit.Droid;
//using Ffmpegkit.Droid;
using YoutubeDownloader; // или ваш namespace
using Android.OS;

public class FFmpegService_Android : IFFmpegService
{
    public async Task<int> ExecuteAsync(string command)
    {
        FFmpegKit.Execute(command);
        
        return 1;
    }

    public string GetPath()
    {
        return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;
    }

    private int ExecuteFFmpegCommand(string command)
    {
        return 0;
    }
}
