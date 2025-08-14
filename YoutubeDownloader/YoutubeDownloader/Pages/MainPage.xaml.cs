using YoutubeDownloader.Models;
using YoutubeDownloader.PageModels;

namespace YoutubeDownloader.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}