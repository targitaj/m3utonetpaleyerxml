using CommunityToolkit.Mvvm.Input;
using YoutubeDownloader.Models;

namespace YoutubeDownloader.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}