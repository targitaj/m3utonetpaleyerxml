using LinqToExcel;
using LinqToExcel.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uma.Eservices.Common;
using Uma.Eservices.DbAccess;
using Uma.Eservices.DbObjects;
using Uma.Eservices.Logic.Features.Localization;

namespace TranslationImporter
{
    public class WebElementImporter
    {
        public GeneralDbDataHelper dbH { get; set; }

        public ExcelQueryFactory excel { get; set; }

        public int UpdateCounter { get; set; }

        public int NewCounter { get; set; }

        public WebElementImporter()
        {
            this.dbH = new GeneralDbDataHelper(new UnitOfWork());
            dbH.Logger = new Mock<ILog>().Object;
        }

        public void ExtractFromExcel(string path)
        {
            ExcelQueryable<WebElementExcelModel> rows = this.GetMainWorkSheet(path);

            foreach (var item in rows.Skip(1))
            {
                if (String.IsNullOrEmpty(item.ModelName))
                {
                    Console.WriteLine("Import failed ----> Model Name is null/empty  ----> Have a nice day");
                    Console.WriteLine("ModelName " + item.ModelName);
                    continue;
                }

                this.CreateWebElemnt(item);
            }

            Console.WriteLine("Total rows:    {0}", rows.Count() - 1);
            Console.WriteLine("Total Updates: {0}", this.UpdateCounter);
            Console.WriteLine("Total added: {0}", this.NewCounter);
        }

        public ExcelQueryable<WebElementExcelModel> GetMainWorkSheet(string path)
        {
            this.excel = new ExcelQueryFactory(path);
            this.excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Jet;

            this.excel.StrictMapping = StrictMappingType.None;

            this.ValidateExcelColumns(excel);

            this.MapColumns(excel);

            // Group by fature colu,m
            var rows = excel.Worksheet<WebElementExcelModel>("Main");

            return rows;
        }

        private void CreateWebElemnt(WebElementExcelModel item)
        {
            List<WebElementTranslation> WebTransList = new List<WebElementTranslation>();

            // In case of linq to excel update
            //this.CreateWebTrans(item.Prompt, WebTransList, TranslatedTextType.Prompt);
            //this.CreateWebTrans(item.ControlText, WebTransList, TranslatedTextType.ControlText);
            //this.CreateWebTrans(item.Help, WebTransList, TranslatedTextType.Help);
            //this.CreateWebTrans(item.ControlText, WebTransList, TranslatedTextType.ControlText);

            #region CreateWebTrans method call

            this.CreateWebTrans(new TextTransLang
              {
                  EngText = item.LabelEngText,
                  FinText = item.LabelFinText,
                  SweText = item.LabelSweText,
              },
              WebTransList, TranslatedTextType.Label);

            this.CreateWebTrans(new TextTransLang
            {
                EngText = item.ControlEngText,
                FinText = item.ControlFinText,
                SweText = item.ControlSweText,
            },
            WebTransList, TranslatedTextType.ControlText);

            this.CreateWebTrans(new TextTransLang
            {
                EngText = item.HelpEngText,
                FinText = item.HelpFinText,
                SweText = item.HelpSweText,
            },
            WebTransList, TranslatedTextType.Help);

            this.CreateWebTrans(new TextTransLang
            {
                EngText = item.EnumEngText,
                FinText = item.EnumFinText,
                SweText = item.EnumSweText,
            },
            WebTransList, TranslatedTextType.EnumText);

            #endregion


            if (WebTransList.Count == 0)
            {
                Console.WriteLine("ERROR  ->  Zero translation in list:   {0}   {1}", item.ModelName, item.PropertyName);
                return;
            }
            this.AddUpdateWebelement(item.ModelName, item.PropertyName, WebTransList);
        }

        private void AddUpdateWebelement(string ModelName, string PropertyName, List<WebElementTranslation> WebTransList)
        {
            WebElement webE = new WebElement
            {
                ModelName = ModelName,
                PropertyName = PropertyName,
                WebElementTranslations = WebTransList
            };

            var dbElem = this.dbH.Get<WebElement>(o => o.ModelName == ModelName & o.PropertyName == PropertyName);

            if (dbElem != null)
            {
                foreach (var item in webE.WebElementTranslations)
                {
                    WebElementTranslation text = dbElem.WebElementTranslations.Where(o => o.TranslationType == item.TranslationType & o.Language == item.Language).FirstOrDefault();
                    item.WebElementId = dbElem.WebElementId;

                    if (text == null)
                    {
                        this.dbH.Create<WebElementTranslation>(item);

                        Console.WriteLine("Create WebElementTranslation");
                        this.dbH.FlushChanges();
                        this.UpdateCounter++;
                    }
                    else
                    {
                        if (item.TranslatedText != text.TranslatedText)
                        {
                            text.TranslatedText = item.TranslatedText;
                            this.dbH.Update<WebElementTranslation>(text);

                            Console.WriteLine("Update WebElementTranslation");
                            this.dbH.FlushChanges();
                            this.UpdateCounter++;
                        }
                    }
                }
            }
            else
            {
                this.dbH.Create<WebElement>(webE);
                this.dbH.FlushChanges();

                Console.WriteLine("Created: " + webE.PropertyName);

                this.NewCounter++;
            }
        }

        private void CreateWebTrans(TextTransLang textTransLang, List<WebElementTranslation> WebTransList, TranslatedTextType translatedTextType)
        {
            if (!string.IsNullOrEmpty(textTransLang.EngText))
            {
                WebTransList.Add(new WebElementTranslation
                {
                    TranslationType = translatedTextType,
                    Language = SupportedLanguage.English,
                    TranslatedText = textTransLang.EngText
                });
                // Console.WriteLine("Add: {0}   {1}", SupportedLanguage.English, translatedTextType);
            }

            if (!string.IsNullOrEmpty(textTransLang.FinText))
            {
                WebTransList.Add(new WebElementTranslation
                {
                    TranslationType = translatedTextType,
                    Language = SupportedLanguage.Finnish,
                    TranslatedText = textTransLang.FinText
                });
                // Console.WriteLine("Add: {0}   {1}", SupportedLanguage.Finnish, translatedTextType);
            }

            if (!string.IsNullOrEmpty(textTransLang.SweText))
            {
                WebTransList.Add(new WebElementTranslation
                {
                    TranslationType = translatedTextType,
                    Language = SupportedLanguage.Swedish,
                    TranslatedText = textTransLang.SweText
                });
                //  Console.WriteLine("Add: {0}   {1}", SupportedLanguage.Swedish, translatedTextType);
            }
        }

        private void MapColumns(ExcelQueryFactory excel)
        {
            var columns = excel.GetColumnNames("Main").ToList();

            #region In caseof  Linq to Excel update

            //excel.AddMapping<WebElementExcelModel>(m => m.ModelName, columns[0]);
            //excel.AddMapping<WebElementExcelModel>(m => m.PropertyName, columns[1]);

            //// Prompt
            //excel.AddMapping<WebElementExcelModel>(m => m.Label.EngText, columns[2]);
            //excel.AddMapping<WebElementExcelModel>(m => m.Label.FinText, columns[3]);
            //excel.AddMapping<WebElementExcelModel>(m => m.Label.SweText, columns[4]);


            //// ControlText
            //excel.AddMapping<WebElementExcelModel>(m => m.ControlText.EngText, columns[5]);
            //excel.AddMapping<WebElementExcelModel>(m => m.ControlText.FinText, columns[6]);
            //excel.AddMapping<WebElementExcelModel>(m => m.ControlText.SweText, columns[7]);

            //// Help
            //excel.AddMapping<WebElementExcelModel>(m => m.Help.EngText, columns[8]);
            //excel.AddMapping<WebElementExcelModel>(m => m.Help.FinText, columns[9]);
            //excel.AddMapping<WebElementExcelModel>(m => m.Help.SweText, columns[10]);

            //// EnumText
            //excel.AddMapping<WebElementExcelModel>(m => m.EnumText.EngText, columns[11]);
            //excel.AddMapping<WebElementExcelModel>(m => m.EnumText.FinText, columns[12]);
            //excel.AddMapping<WebElementExcelModel>(m => m.EnumText.SweText, columns[13]); 

            #endregion

            excel.AddMapping<WebElementExcelModel>(m => m.ModelName, columns[0]);
            excel.AddMapping<WebElementExcelModel>(m => m.PropertyName, columns[1]);

            // Label
            excel.AddMapping<WebElementExcelModel>(m => m.LabelEngText, columns[2]);
            excel.AddMapping<WebElementExcelModel>(m => m.LabelFinText, columns[3]);
            excel.AddMapping<WebElementExcelModel>(m => m.LabelSweText, columns[4]);

            // ControlText
            excel.AddMapping<WebElementExcelModel>(m => m.ControlEngText, columns[5]);
            excel.AddMapping<WebElementExcelModel>(m => m.ControlFinText, columns[6]);
            excel.AddMapping<WebElementExcelModel>(m => m.ControlSweText, columns[7]);

            // Help
            excel.AddMapping<WebElementExcelModel>(m => m.HelpEngText, columns[8]);
            excel.AddMapping<WebElementExcelModel>(m => m.HelpFinText, columns[9]);
            excel.AddMapping<WebElementExcelModel>(m => m.HelpSweText, columns[10]);

            // EnumText                                   
            excel.AddMapping<WebElementExcelModel>(m => m.EnumEngText, columns[11]);
            excel.AddMapping<WebElementExcelModel>(m => m.EnumFinText, columns[12]);
            excel.AddMapping<WebElementExcelModel>(m => m.EnumSweText, columns[13]);


        }

        private void ValidateExcelColumns(ExcelQueryFactory excel)
        {
            List<string> OriginalColumnNames = new List<string>() 
            {
                "ModelName", "PropertyName", "English","Finnish","Swedish","English1", "Finnish1","Swedish1","English2",
                "Finnish2", "Swedish2", "English3" , "Finnish3", "Swedish3"
            };

            var excelcolumns = excel.GetColumnNames("Main").ToList();
            for (int i = 0; i < OriginalColumnNames.Count(); i++)
            {
                if (excelcolumns[i] != OriginalColumnNames[i])
                {
                    throw new ArgumentException("Column names have been changed or changed order!!!");
                }
            }
        }
    }
}

/* Column names ======  */
/* [0]: "ModelName"
   [1]: "PropertyName"
   [2]: "English"  -> Prompt
   [3]: "Finnish"
   [4]: "Swedish"
   [5]: "English1" -> ControlText
   [6]: "Finnish1"
   [7]: "Swedish1"
   [8]: "English2" -> Help
   [9]: "Finnish2"
   [10]: "Swedish2"
   [11]: "English3" -> EnumText
   [12]: "Finnish3"
   [13]: "Swedish3"
*/
