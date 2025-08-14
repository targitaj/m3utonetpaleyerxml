using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace YoutubeDownloader;

[Service(
    Exported = false,
    ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync
)]
public class DownloadForegroundService : Service
{
    // Intent actions / extras
    public const string ActionStart = "ACTION_START_DOWNLOAD";
    public const string ActionCancel = "ACTION_CANCEL_DOWNLOAD";
    public const string ExtraUrl = "EXTRA_URL";
    public const string ExtraIsMax = "EXTRA_IS_MAX";

    // Notifications
    public const string ChannelId = "downloader_channel";
    private const int NotifId = 1001;
    private const int NotifDoneId = 1002;
    private const int NotifErrorId = 1003;

    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public override IBinder? OnBind(Intent intent) => null;

    public override void OnCreate()
    {
        base.OnCreate();
        EnsureNotificationChannel();
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        //var notif = BuildNotification(title: "Загрузка видео", text: "Подготовка…", progress: null, ongoing: true);
        //StartForeground(NotifId, notif);
        //return base.OnStartCommand(intent, flags, startId);
        if (intent == null)
            return StartCommandResult.NotSticky;

        var action = intent.Action;

        if (action == ActionCancel)
        {
            _cts?.Cancel();
            return StartCommandResult.NotSticky;
        }

        if (action == ActionStart)
        {
            if (_isRunning)
            {
                // Уже идёт — игнор
                return StartCommandResult.Sticky;
            }

            var url = intent.GetStringExtra(ExtraUrl) ?? string.Empty;
            var isMax = intent.GetBooleanExtra(ExtraIsMax, true);

            _cts = new CancellationTokenSource();
            _isRunning = true;

            // Поставим базовое «вечное» уведомление
            var notif = BuildNotification(title: "Загрузка видео", text: "Подготовка…", progress: null, ongoing: true);
            StartForeground(NotifId, notif);

            _ = Task.Run(() => RunAsync(url, isMax, _cts!.Token));
            return StartCommandResult.Sticky;
        }

        return StartCommandResult.NotSticky;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try { _cts?.Cancel(); } catch { }
        _cts?.Dispose();
        _cts = null;
        _isRunning = false;
        base.OnDestroy();
    }

    // === Core ===

    private async Task RunAsync(string url, bool isMax, CancellationToken ct)
    {
        try
        {
            // Разруливаем зависимости.
            // Предполагается, что ты где-то в MauiProgram.cs сделал:
            // ServiceLocator.Provider = app.Services;
            var ffmpegSvc = ServiceLocator.Resolve<IFFmpegService>();
            var worker = new MauiDownloadWorker(ffmpegSvc);

            // Коллбек прогресса → нотификация
            void Report(double p)
            {
                UpdateProgressNotification(p, $"Загрузка… {(int)(p * 100)}%");
            }

            // Статусные сообщения (опционально)
            void Status(string s)
            {
                UpdateProgressNotification(null, s);
            }

            Status("Старт…");
            await worker.DownloadAsync(url, isMax, Report, Status, ct);

            // Готово
            var done = BuildNotification("Загрузка завершена", "Файл сохранён", null, ongoing: false);
            NotificationManagerCompat.From(this).Notify(NotifDoneId, done);
        }
        catch (System.OperationCanceledException)
        {
            var n = BuildNotification("Загрузка отменена", "Операция остановлена", null, ongoing: false);
            NotificationManagerCompat.From(this).Notify(NotifErrorId, n);
        }
        catch (Exception ex)
        {
            var n = BuildNotification("Ошибка загрузки", Shorten(ex.Message, 120), null, ongoing: false);
            NotificationManagerCompat.From(this).Notify(NotifErrorId, n);
        }
        finally
        {
            try { StopForeground(StopForegroundFlags.Remove); } catch { }
            StopSelf();
            _isRunning = false;
        }
    }

    // === Notification helpers ===

    private void EnsureNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var mgr = (NotificationManager?)GetSystemService(NotificationService);
        if (mgr is null) return;

        var ch = new NotificationChannel(ChannelId, "Downloads", NotificationImportance.Low)
        {
            Description = "Прогресс загрузок",
            LockscreenVisibility = NotificationVisibility.Secret
        };
        mgr.CreateNotificationChannel(ch);
    }

    private Notification BuildNotification(string title, string text, int? progress, bool ongoing)
    {
        // Используем системную иконку, чтобы не требовать свой ресурс
        var builder = new NotificationCompat.Builder(this, ChannelId)
            .SetContentTitle(title)
            .SetContentText(text)
            .SetSmallIcon(Android.Resource.Drawable.StatSysDownload)
            .SetOnlyAlertOnce(true)
            .SetOngoing(ongoing)
            .SetPriority((int)NotificationPriority.Low);

        if (progress.HasValue)
            builder.SetProgress(100, progress.Value, false);

        // Добавим действие «Отмена»
        var cancelIntent = new Intent(this, typeof(DownloadForegroundService)).SetAction(ActionCancel);
        var cancelPi = PendingIntent.GetService(this, 1, cancelIntent, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);
        builder.AddAction(0, "Отмена", cancelPi);

        return builder.Build();
    }

    private void UpdateProgressNotification(double? progress01, string text)
    {
        int? p = progress01.HasValue ? Math.Clamp((int)(progress01.Value * 100), 0, 100) : (int?)null;
        var n = BuildNotification("Загрузка видео", text, p, ongoing: true);
        NotificationManagerCompat.From(this).Notify(NotifId, n);
    }

    private static string Shorten(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return s.Length <= max ? s : s.Substring(0, max) + "…";
    }
}
