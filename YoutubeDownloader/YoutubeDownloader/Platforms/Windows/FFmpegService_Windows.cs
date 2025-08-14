using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.Platforms.Windows
{
    internal class FFmpegService_Windows : IFFmpegService
    {
        public Task<int> ExecuteAsync(string command)
        {
            throw new NotImplementedException();
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }
    }
}
