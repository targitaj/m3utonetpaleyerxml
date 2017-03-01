using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using IniParser;
using IniParser.Parser;
using MailRuCloudApi;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;

namespace AM.MailRuLinkCreator.MainViewModel
{
    public class MainViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        #region Members

        private string _directoryPath;// = @"C:\Users\dron\Mail.Ru\Цех №1";
        private string _loginName;// = "testAMApi";
        private string _password;// = "devTest1";
        private string _rootDirectory;
        private int _qrCodeModuleSize;
        private int _filePrice;
        private NavigationService _navigationService;

        #endregion

        #region Commands

        public DelegateCommand StartCommand => new DelegateCommand(Start);

        #endregion

        #region Properties

        public string LoginName
        {
            get { return _loginName; }
            set { SetProperty(ref _loginName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { SetProperty(ref _directoryPath, value); }
        }

        public List<string> Images { get; set; } = new List<string>();

        #endregion

        public MainViewModel(NavigationService navigationService)
        {
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
            _navigationService = navigationService;

            Enumerable.Repeat("100_2102.jpg", 100000).ForEach(Images.Add);
        }

        #region Methods



        public async void StartAsync()
        {
            var account = new Account(LoginName, Password);
            var api = new MailRuCloud() { Account = account };

            DirectoryInfo di = new DirectoryInfo(DirectoryPath);
            var childDirectories = di.GetDirectories();
            var resultDocument = new Document();
            resultDocument.Sections.Add(new Section(resultDocument));
            //resultDocument.Sections.Add(new Body(new Document()))
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

                    template.Replace("%TotalCost%", (childDirectory.GetFiles().Length*_filePrice).ToString(), false, true);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                        var image = Image.FromStream(stream);

                        TextSelection selection = template.FindString("%CloudQRCode%", true, true);

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
            _navigationService.NavigateToRegion(Regions.MainViewRegion, Views.MainView);
            //Thread th = new Thread(StartAsync);
            //th.Start();
        }

        public bool KeepAlive => true;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion
    }
}
