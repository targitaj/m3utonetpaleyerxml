using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using IniParser;
using IniParser.Parser;
using MailRuCloudApi;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;

namespace AM.MailRuLinkCreator.MainViewModel
{
    public class MainViewModel : BindableBase
    {
#region Members

        private string _rootDirectoryPath;
        private string _directoryPath;// = @"C:\Users\dron\Mail.Ru\Цех №1";
        private string _loginName;// = "testAMApi";
        private string _password;// = "devTest1";
        private string _rootDirectory;
        private int _qrCodeModuleSize;

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

        public string RootDirectoryPath
        {
            get { return _rootDirectoryPath; }
            set { SetProperty(ref _rootDirectoryPath, value); }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { SetProperty(ref _directoryPath, value); }
        }

        #endregion

        public MainViewModel()
        {
            var iniParser = new FileIniDataParser();
            var ini = iniParser.ReadFile("Settings.ini");
            var settings = ini.Sections["settings"];
            _rootDirectory = settings["CloudLocalPath"];
            _qrCodeModuleSize = int.Parse(settings["QRCodeModuleSize"]);
        }

        #region Methods



        public async void StartAsync()
        {
            var account = new Account(LoginName, Password);
            var api = new MailRuCloud() { Account = account };

            DirectoryInfo di = new DirectoryInfo(DirectoryPath);
            var childDirectories = di.GetDirectories();
            var resultDocument = new Document();
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
                        resultDocument.Sections.Add(section.Clone());
                    }
                }
            }

            resultDocument.SaveToFile("result.doc");
        }

        public void Start()
        {
            Thread th = new Thread(StartAsync);
            th.Start();
        }

        #endregion
    }
}
