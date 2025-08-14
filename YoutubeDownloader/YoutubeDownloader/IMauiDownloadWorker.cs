using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader
{
    public interface IMauiDownloadWorker
    {
        Task DownloadAndMergeAsync(string videoUrl, bool isMax, Action<double> reportProgress, CancellationToken ct);
    }
}
