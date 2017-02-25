using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using IniParser;
using MailRuCloudApi;
using Spire.Doc;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using Application = System.Windows.Application;
using File = System.IO.File;
using MessageBox = System.Windows.MessageBox;
using Section = System.Windows.Documents.Section;

namespace AM.MailRuLinkCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string fileName = "ERROR" + DateTime.Now.ToString("o").Replace(":", ".") + ".txt";

            MessageBox.Show("Произошла ошибка, проверьте настройки, введенные данные. Детали ошибки записаны в файл " + fileName);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName, GetErrorData((Exception)e.ExceptionObject));

            Environment.Exit(0);
        }

        private string GetErrorData(Exception exception)
        {
            var res = exception.Message + Environment.NewLine + exception.StackTrace;

            if (exception.InnerException != null)
            {
                res += Environment.NewLine + GetErrorData(exception.InnerException);
            }

            return res;
        }

        #region Members

        private string _directoryPath;// = @"C:\Users\dron\Mail.Ru\Цех №1";
        private string _loginName;// = "testAMApi";
        private string _password;// = "devTest1";
        private string _rootDirectory;
        private int _qrCodeModuleSize;
        private int _filePrice;

        #endregion

        #region Commands

        //public DelegateCommand StartCommand => new DelegateCommand(Start);

        #endregion

        #region Properties

        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        public string Password
        {
            get { return _password; }
            set {  _password = value; }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        #endregion
        
        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var iniParser = new FileIniDataParser();
            var ini = iniParser.ReadFile("Settings.ini");
            var settings = ini.Sections["settings"];
            _rootDirectory = settings["CloudLocalPath"];
            _qrCodeModuleSize = int.Parse(settings["QRCodeModuleSize"]);
            DirectoryPath = _rootDirectory;
            LoginName = settings["Login"];
            Password = settings["Password"];
            _filePrice = int.Parse(settings["FilePrice"]);

            var dialog = new FolderBrowserDialog();
            var currentPath = DirectoryPath;
            dialog.SelectedPath = currentPath;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DirectoryPath = dialog.SelectedPath;
                Start();
            }
        }

        public async void StartAsync()
        {
            var account = new Account(LoginName, Password);
            var api = new MailRuCloud() { Account = account };

            DirectoryInfo di = new DirectoryInfo(DirectoryPath);
            var childDirectories = di.GetDirectories();
            var resultDocument = new Document();
            resultDocument.Sections.Add(new Spire.Doc.Section(resultDocument));
            api.Account.CheckAuth();

            foreach (var childDirectory in childDirectories)
            {
                var folder = new Folder()
                {
                    Name = Uri.EscapeDataString(childDirectory.Name.Replace(@"\", "/")),
                    FulPath = Uri.EscapeDataString(childDirectory.FullName.Replace(_rootDirectory, "").Replace(@"\", "/"))
                };

                var res = await api.GetPublishLink(folder);

                using (FileStream fs = System.IO.File.OpenRead("template.doc"))
                {
                    var template = new Document(fs);
                    template.Replace("%FolderName%", di.Name, false, true);
                    template.Replace("%SubFolderName%", childDirectory.Name, false, true);
                    template.Replace("%NumberOfFiles%", childDirectory.GetFiles().Length.ToString(), false, true);
                    template.Replace("%Files%",
                        string.Join(Environment.NewLine, childDirectory.GetFiles().Select(s => s.Name)), false,
                        true);

                    template.Replace("%CloudURL%", res, false, true);

                    QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    QrCode qrCode = qrEncoder.Encode(res);
                    GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(_qrCodeModuleSize, QuietZoneModules.Two));

                    template.Replace("%TotalCost%", (childDirectory.GetFiles().Length * _filePrice).ToString(), false, true);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                        var image = Image.FromStream(stream);

                        Spire.Doc.Documents.TextSelection selection = template.FindString("%CloudQRCode%", true, true);

                        DocPicture pic = new DocPicture(template);
                        pic.LoadImage(image);

                        var range = selection.GetAsOneRange();
                        var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                        range.OwnerParagraph.ChildObjects.Insert(index, pic);
                        range.OwnerParagraph.ChildObjects.Remove(range);
                    }

                    foreach (ISection section in template.Sections)
                    {

                        foreach (DocumentObject childObject in section.Body.ChildObjects)
                        {
                            resultDocument.Sections[0].Body.ChildObjects.Add(childObject.Clone());
                        }
                    }
                }
            }

            resultDocument.SaveToFile("result.doc");

            Process p = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.UseShellExecute = true;
            pi.FileName = @"result.doc";
            p.StartInfo = pi;
            p.Start();

            Environment.Exit(0);
        }

        public void Start()
        {
            Thread th = new Thread(StartAsync);
            th.Start();
        }

        #endregion
    }
}
