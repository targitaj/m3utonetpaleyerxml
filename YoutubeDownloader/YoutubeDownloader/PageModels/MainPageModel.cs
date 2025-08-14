//using Android.OS;
//using Android.Widget;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Net.Http.Headers;
using System.Net;
using YoutubeDownloader.Models;
//using YoutubeExplode;
//using YoutubeExplode.Videos.Streams;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using VideoLibrary;
using Microsoft.Maui.ApplicationModel;
#if ANDROID
using Android;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
#endif

namespace YoutubeDownloader.PageModels
{
    public partial class MainPageModel : ObservableObject, IProjectTaskPageModel
    {
        private bool _isNavigatedTo;
        private bool _dataLoaded;
        private readonly ProjectRepository _projectRepository;
        private readonly TaskRepository _taskRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ModalErrorHandler _errorHandler;
        private readonly SeedDataService _seedDataService;
        private readonly IFFmpegService _ffmpegService;

        [ObservableProperty]
        private List<CategoryChartData> _todoCategoryData = [];

        [ObservableProperty]
        private List<Brush> _todoCategoryColors = [];

        [ObservableProperty]
        private List<ProjectTask> _tasks = [];

        [ObservableProperty]
        private List<Project> _projects = [];

        [ObservableProperty]
        bool _isBusy;

        [ObservableProperty]
        bool _isRefreshing;

        //[ObservableProperty]
        //private string _asdasd = "hahahah";

        //[ObservableProperty]
        //private string _today = DateTime.Now.ToString("dddd, MMM d");

        [ObservableProperty] private string _videoUrl;// = "https://www.youtube.com/watch?v=N5pXme-h444&ab_channel=%D0%A1%D0%B5%D1%80%D0%B3%D0%B5%D0%B9%D0%9C%D0%B0%D1%80%D1%86%D0%B8%D0%BD%D0%BA%D0%B5%D0%B2%D0%B8%D1%87";

        [ObservableProperty]
        private double _progress;

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private bool _isMaxResolution = true;

        public bool HasCompletedTasks
            => Tasks?.Any(t => t.IsCompleted) ?? false;

        public MainPageModel(SeedDataService seedDataService, ProjectRepository projectRepository,
            TaskRepository taskRepository, CategoryRepository categoryRepository, ModalErrorHandler errorHandler, IFFmpegService ffmpegService)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _errorHandler = errorHandler;
            _seedDataService = seedDataService;
            _ffmpegService = ffmpegService;

             // или Xamarin.Essentials в зависимости от проекта

            // Запрос разрешения на чтение внешнего хранилища
            var status = Permissions.RequestAsync<Permissions.StorageRead>().Result;

            if (status != PermissionStatus.Granted)
            {
                UpdateStatus("нет доступа");
                return;
            }

            status = Permissions.RequestAsync<Permissions.StorageWrite>().Result;

            if (status != PermissionStatus.Granted)
            {
                UpdateStatus("нет доступа");
                return;
            }
        }

        private async Task LoadData()
        {
            try
            {
                IsBusy = true;

                Projects = await _projectRepository.ListAsync();

                var chartData = new List<CategoryChartData>();
                var chartColors = new List<Brush>();

                var categories = await _categoryRepository.ListAsync();
                foreach (var category in categories)
                {
                    chartColors.Add(category.ColorBrush);

                    var ps = Projects.Where(p => p.CategoryID == category.ID).ToList();
                    int tasksCount = ps.SelectMany(p => p.Tasks).Count();

                    chartData.Add(new(category.Title, tasksCount));
                }

                TodoCategoryData = chartData;
                TodoCategoryColors = chartColors;

                Tasks = await _taskRepository.ListAsync();
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(HasCompletedTasks));
            }
        }

        public bool IsAndroid
        {
            get
            {
#if ANDROID
                return true;
#endif

                return false;
            }
        }

        private async Task InitData(SeedDataService seedDataService)
        {
            bool isSeeded = Preferences.Default.ContainsKey("is_seeded");

            if (!isSeeded)
            {
                await seedDataService.LoadSeedDataAsync();
            }

            Preferences.Default.Set("is_seeded", true);
            await Refresh();
        }

        public void UpdateStatus(string text)
        {
            Status = text;
            //var handler = new Android.OS.Handler(Looper.MainLooper); // Создание обработчика для главного потока
            ////ApplicationContext.ApplicationContext
            //handler.Post(() =>
            //{
            //    TextView textView = FindViewById<TextView>(Resource.Id.textView);

            //    textView.Text = text;
            //});
        }

        private string GetPath()
        {
            if (IsAndroid)
            {
                return _ffmpegService.GetPath();
            }

            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
        }

        [RelayCommand]
        private async Task Download()
        {
            #if ANDROID
    // Android 13+: спросите POST_NOTIFICATIONS если нужно показывать прогресс
    //if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu) // API 33
    //        {
    //            var ctx1 = Android.App.Application.Context;
    //            if (ContextCompat.CheckSelfPermission(ctx1, Manifest.Permission.PostNotifications) != Permission.Granted)
    //            {
    //                var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
    //                ActivityCompat.RequestPermissions(activity!, new[] { Manifest.Permission.PostNotifications }, 0);
    //            }
    //        }
            // (обрабатывайте отказ по желанию)

            

            try
            {
                var ctx = Android.App.Application.Context;

                var intent = new Android.Content.Intent(ctx, typeof(DownloadForegroundService))
                    .SetAction(DownloadForegroundService.ActionStart)
                    .PutExtra(DownloadForegroundService.ExtraUrl, _videoUrl)
                    .PutExtra(DownloadForegroundService.ExtraIsMax, _isMaxResolution);
                ctx.StartForegroundService(intent);
            }
            catch (Exception e)
            {
                UpdateStatus(e.Message);
            }
            //ctx.StartForegroundService(intent);
#else

            UpdateStatus($"var youtube = new YoutubeClient();");

            try
            {
                var youTube = YouTube.Default; // starting point for YouTube actions
                UpdateStatus("youTube.GetVideoAsync(_videoUrl);" );
                var all = await youTube.GetAllVideosAsync(_videoUrl);
                var ffmpegPath = Path.Combine(GetPath(), "ffmpeg");

                if (!IsAndroid && !File.Exists(ffmpegPath))
                {
                    UpdateStatus($"Download ffmpeg");
                    if (!IsAndroid)
                    {
                        await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full, ffmpegPath,
                            new Progress<ProgressInfo>(ddd => { UpdateProgress(ddd.DownloadedBytes / ddd.TotalBytes); }));
                    }
                }

                UpdateStatus("videoTask.AsTask().Wait();");
                string sanitizedTitle = Guid.NewGuid().ToString();

                UpdateStatus("GetManifestAsync");
                string downloadsDir = Path.Combine(GetPath(), "youtube");
                if (!Directory.Exists(downloadsDir))
                {
                    Directory.CreateDirectory(downloadsDir);
                }

                string videoName = Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");//Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");
                var vidUrl = string.Empty;
                if (!_isMaxResolution)
                {
                      var vid = all.OrderBy(o => o.Resolution)
                        .First(f => f.Format == VideoFormat.Mp4 && f.Resolution >= 360);

                      vidUrl = vid.Uri;
                }
                else
                {
                    var vid = all.OrderByDescending(o => o.Resolution)
                        .First(f => f.Format == VideoFormat.Mp4);

                    vidUrl = vid.Uri;
                }
                UpdateStatus("download video");
                await DownloadFileWithResumeAsync(vidUrl, videoName);

                string audName = Path.Combine(downloadsDir, $"{Guid.NewGuid()}.mp4");
                string fileName = Path.Combine(downloadsDir, $"{sanitizedTitle}.mp4");
                var audUrl = all.OrderByDescending(o => o.AudioBitrate).First().Uri;
                UpdateStatus("download audio");
                await DownloadFileWithResumeAsync(audUrl, audName);
                string command = $"-i \"{videoName}\" -i \"{audName}\" -shortest {fileName}";
                UpdateStatus($"Merging audio and video");

                if (!IsAndroid)
                {
                    FFmpeg.SetExecutablesPath(ffmpegPath);
                    await new Conversion().Start(command);
                }
                else
                {
                    await _ffmpegService.ExecuteAsync(command);
                }
                    
                UpdateStatus($"Done");
            }
            catch (Exception e)
            {
                UpdateStatus(e.Message + System.Environment.NewLine + e.InnerException?.Message);
            }
#endif
        }

        public async Task DownloadFileWithResumeAsync(string downloadUrl, string filePath)
        {


            try
            {
                //if (Directory.Exists())
                long offset = 0;
                if (System.IO.File.Exists(filePath))
                {
                    offset = new FileInfo(filePath).Length;
                }

                // Формируем запрос
                var request = new HttpRequestMessage(HttpMethod.Get, downloadUrl);

                // Если что-то уже скачали — добавляем Range-заголовок
                if (offset > 0)
                {
                    // "bytes={уже загружено}-"
                    request.Headers.Range = new RangeHeaderValue(offset, null);
                }

                using (var httpClient = new HttpClient())
                {
                    // Запрашиваем заголовки и поток для чтения контента
                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // Если сервер сообщил, что запрошенный Range недопустим (например, файл уже полностью скачан)
                        if (response.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
                        {
                            Console.WriteLine("Файл уже полностью скачан или запрошенный диапазон недопустим.");
                            return;
                        }

                        response.EnsureSuccessStatusCode();

                        // Размер оставшейся (или полной, если не поддерживается range) части
                        var contentLength = response.Content.Headers.ContentLength ?? 0L;

                        // Текущее количество байт, которое будет считаться загруженным
                        // (если оффсет уже существовал, то общее "продвижение" будем считать с учётом offset)
                        long totalRead = offset;

                        byte[] buffer = new byte[8192];
                        bool isMoreToRead = true;
                        string content = "Привет из MAUI!";
                        //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        //filePath = Path.Combine(folderPath, "myFile.txt");
                        // Асинхронная запись текста в файл
                        //await File.WriteAllTextAsync(filePath, content);
                        // Открываем файл в режиме дозаписи (Append), чтобы не затирать ранее загруженную часть
                        //var fileStream111 = new FileStream("1.txt", FileMode.CreateNew);
                        //await Task.Run(async () =>
                        //{
                        //    await using (var fileStream = new FileStream("1.txt", FileMode.Append, FileAccess.Write,
                        //                     FileShare.None, 8192, useAsync: true))
                        //    {
                        //        await using (var stream = await response.Content.ReadAsStreamAsync())
                        //        {
                        //            while (isMoreToRead)
                        //            {
                        //                int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        //                if (read == 0)
                        //                {
                        //                    // дочитали всё, выходим
                        //                    isMoreToRead = false;
                        //                    continue;
                        //                }

                        //                // Пишем в файл то, что только что скачали
                        //                await fileStream.WriteAsync(buffer, 0, read);

                        //                totalRead += read;

                        //                // Обновляем прогресс (пример условной логики).
                        //                // Если мы возобновляем загрузку с offset, то полный размер файла может быть offset + contentLength
                        //                // или сервер может возвращать ContentRange – при необходимости обрабатываем это.
                        //                double progress = (totalRead * 1.0 / (offset + contentLength)) * 100.0;
                        //                UpdateProgress(progress);
                        //            }
                        //        }
                        //    }
                        //});
                        await using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write,
                                         FileShare.None, 8192, useAsync: true))
                        {
                            await using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                while (isMoreToRead)
                                {
                                    int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                                    if (read == 0)
                                    {
                                        // дочитали всё, выходим
                                        isMoreToRead = false;
                                        continue;
                                    }

                                    // Пишем в файл то, что только что скачали
                                    await fileStream.WriteAsync(buffer, 0, read);

                                    totalRead += read;

                                    // Обновляем прогресс (пример условной логики).
                                    // Если мы возобновляем загрузку с offset, то полный размер файла может быть offset + contentLength
                                    // или сервер может возвращать ContentRange – при необходимости обрабатываем это.
                                    double progress = (totalRead * 1.0 / (offset + contentLength)) * 100.0;
                                    UpdateProgress(progress);
                                }
                            }
                        }

                    }
                }

                Console.WriteLine("Файл успешно загружен/дозагружен!");
            }
            catch (Exception e)
            {
                tryCount++;
                UpdateStatus("popitka " + e.Message + tryCount);
                await DownloadFileWithResumeAsync(downloadUrl, filePath);
            }
            // Определяем, сколько уже было загружено

        }

        private int tryCount = 0;

        public void UpdateProgress(double progress)
        {
            Progress = progress / 100;
            //var handler = new Android.OS.Handler(Looper.MainLooper); // Создание обработчика для главного потока
            ////ApplicationContext.ApplicationContext
            //handler.Post(() =>
            //{
            //    ProgressBar pr = FindViewById<ProgressBar>(Resource.Id.progressBar);
            //    pr.Progress = (int)progress;
            //});

            //// Обновите ваш прогресс-бар или другой UI элемент здесь
            //// Например, отправляя значение в ProgressBar через MessagingCenter или используя MVVM для обновления
            //Console.WriteLine($"Download progress: {progress}%");
        }

        [RelayCommand]
        private async Task Refresh()
        {
            try
            {
                IsRefreshing = true;
                await LoadData();
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private void NavigatedTo() =>
            _isNavigatedTo = true;

        [RelayCommand]
        private void NavigatedFrom() =>
            _isNavigatedTo = false;

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_dataLoaded)
            {
                await InitData(_seedDataService);
                _dataLoaded = true;
                await Refresh();
            }
            // This means we are being navigated to
            else if (!_isNavigatedTo)
            {
                await Refresh();
            }
        }

        [RelayCommand]
        private Task TaskCompleted(ProjectTask task)
        {
            OnPropertyChanged(nameof(HasCompletedTasks));
            return _taskRepository.SaveItemAsync(task);
        }

        [RelayCommand]
        private Task AddTask()
            => Shell.Current.GoToAsync($"task");

        [RelayCommand]
        private Task NavigateToProject(Project project)
            => Shell.Current.GoToAsync($"project?id={project.ID}");

        [RelayCommand]
        private Task NavigateToTask(ProjectTask task)
            => Shell.Current.GoToAsync($"task?id={task.ID}");

        [RelayCommand]
        private async Task CleanTasks()
        {
            var completedTasks = Tasks.Where(t => t.IsCompleted).ToList();
            foreach (var task in completedTasks)
            {
                await _taskRepository.DeleteItemAsync(task);
                Tasks.Remove(task);
            }

            OnPropertyChanged(nameof(HasCompletedTasks));
            Tasks = new(Tasks);
            await AppShell.DisplayToastAsync("All cleaned up!");
        }
    }
}