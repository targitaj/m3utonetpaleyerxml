using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader
{
    public interface IFFmpegService
    {
        Task<int> ExecuteAsync(string command);

        string GetPath();
    }
}
