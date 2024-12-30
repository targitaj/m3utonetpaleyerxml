using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Java.Lang;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android;
using Java.Util;
using Exception = System.Exception;

[Service]
public class MyService : Service
{
    const string TAG = "MyService";
    const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    private FtpWebRequest request;
    public override void OnCreate()
    {
        base.OnCreate();
        
        //Log.Info(TAG, "Service created");
    }

    public  void UploadTextToFtp(string textData)
    {
        request = (FtpWebRequest)WebRequest.Create("ftp://192.168.88.34:21/artom1.txt");
        request.Method = WebRequestMethods.Ftp.UploadFile; // Метод загрузки файла
        request.Credentials = new NetworkCredential("screen", "screen");
        request.UsePassive = true;
        request.UseBinary = true;
        request.KeepAlive = false;
        //request.Timeout = 15000;

        // Конвертация текста в массив байтов
        byte[] textBytes = Encoding.UTF8.GetBytes(textData);

        // Установка длины содержимого запроса
        request.ContentLength = textBytes.Length;

        // Запись текстовых данных в поток запроса
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(textBytes, 0, textBytes.Length);
            requestStream.Close();
        }
    }

    private List<string> _data = new List<string>();
    private Thread th;
    private bool? _isScreenOn;

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        CreateNotificationChannel();
        var notification = new Notification.Builder(this, "10111")
            .SetContentTitle("Foreground Service")
            .SetContentText("This is a running foreground service")
            .SetSmallIcon(Resource.Mipmap.SymDefAppIcon)
            .Build();

        StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

        th = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    if (_data.Count > 10)
                    {
                        _data.RemoveAt(0);
                    }

                    var isScreenOn = IsScreenOn();
                    if (_isScreenOn != isScreenOn)
                    {
                        _isScreenOn = isScreenOn;
                        _data.Add(DateTime.Now.ToString("s") + $": {(isScreenOn ? "Screen On" : "Screen Off")}");
                        UploadTextToFtp(string.Join("\r\n", _data));
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        //_data.Add(DateTime.Now.ToString("s") + $": {e.Message}");
                        UploadTextToFtp(string.Join("\r\n", _data));
                    }
                    catch
                    {
                        
                    }
                }

                Thread.Sleep(1000);
            }
        });

        th.Start();

        return StartCommandResult.Sticky;
    }

    public bool IsScreenOn()
    {
        PowerManager powerManager = (PowerManager)GetSystemService(PowerService);
        if (Build.VERSION.SdkInt >= BuildVersionCodes.KitkatWatch)
        {
            return powerManager.IsInteractive;
        }
        else
        {
            return powerManager.IsScreenOn;
        }
    }

    void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel("10111", "Foreground Service", NotificationImportance.Default)
            {
                Description = "Foreground Service Notifications"
            };
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        //Log.Info(TAG, "Service destroyed");
    }
}