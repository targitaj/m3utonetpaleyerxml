using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;
using Uma.Eservices.Logic;
using Uma.Eservices.Models;
using Uma.Eservices.Logic.Features.Localization;
using Microsoft.Practices.Unity;
using Uma.Eservices.DbAccess;
using Uma.Eservices.Models.Localization;
using Uma.Eservices.Common;
using System.IO;

// error: "'microsoft.ace.oledb.12.0' provider is not registered on the local machine"
// solution: http://social.msdn.microsoft.com/Forums/en-US/1d5c04c7-157f-4955-a14b-41d912d50a64/how-to-fix-error-the-microsoftaceoledb120-provider-is-not-registered-on-the-local-machine?forum=vstsdb 
// s2: http://social.msdn.microsoft.com/Forums/en-US/f11b2df9-fd0a-4528-987f-f95dfdccee0a/microsoftaceoledb120-provider-is-not-registered-on-the-local-machine-error?forum=adodotnetdataproviders
//
//TO RUN:
// Update connection String
// Update excel Path

// Remember --->  in excel row / column start with index 1 (and first row is header that also counts)
// That is why all indexes from list are mapped +1 to excel 

namespace TranslationImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            // import ->  from Excel to DB
            // export  from DB to Excell
            // Default values
            string path = string.Empty;
            TranslationType transType = TranslationType.OriginalText;
            ActionType importExport = ActionType.Import;

            if (args.Length > 0)
            {
                //(Type) Enum.Parse(typeof(Type), value);
                //Get excel file from args
                path = args[0];
                transType = (TranslationType)Enum.Parse(typeof(TranslationType), args[0]);
                importExport = (ActionType)Enum.Parse(typeof(ActionType), args[1]);
            }
            else
            {
                path = @"C:\Users\Valdis.Logins\Desktop\eService_Original_text_translation.xlsx";
                //eService_Original_text_translation.xlsx";
                //Kopio eService_Original_text_translation.xlsx";
                //path = @"C:\Users\Valdis.Logins\Desktop\WebElementTranslation.xlsx";


                transType = TranslationType.OriginalText;
                importExport = ActionType.Import;
            }

            // validate file path
            FileInfo ff = new FileInfo(path);
            if (!ff.Exists)
            {
                throw new ArgumentException("File does not exist");
            }

            switch (transType)
            {
                case TranslationType.OriginalText:
                    switch (importExport)
                    {
                        case ActionType.Import:
                            TextImporter pp = new TextImporter();
                            pp.ExtractFromExcel(path);

                            break;
                        case ActionType.Export:
                            TextExporter exp = new TextExporter();
                            exp.ExtractFromDB(path);
                            break;
                    }
                    break;

                case TranslationType.WebElement:
                    switch (importExport)
                    {
                        case ActionType.Import:
                            WebElementImporter pp2 = new WebElementImporter();
                            pp2.ExtractFromExcel(path);
                            break;
                        case ActionType.Export:
                            WebElementExporter exp = new WebElementExporter();
                            exp.ExtractFromDB(path);
                            break;
                    }
                    break;
            }

            Console.WriteLine("Done!:::");
            Console.ReadLine();

        }
    }
}
