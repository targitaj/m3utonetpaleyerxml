using LinqToExcel;
using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uma.Eservices.Common;
using Uma.Eservices.DbAccess;
using db = Uma.Eservices.DbObjects;
using Uma.Eservices.Logic.Features.Localization;
using Uma.Eservices.Models.Localization;
using LinqToExcel.Query;
using System.Threading;

namespace TranslationImporter
{
    public class TextImporter
    {
        public ILocalizationEditor LocalManager { get; set; }

        public GeneralDbDataHelper dbH { get; set; }

        public ExcelQueryFactory excel { get; set; }

        public string OrgTId { get { return "Id"; } }
        public string Original { get { return "Original"; } }
        public string Feature { get { return "Feature"; } }
        public string EnglishText { get { return "English text"; } }
        public string FinnishText { get { return "Finish text"; } }
        public string SwedishText { get { return "Swedish text"; } }

        public int UpdateCounter { get; set; }

        public TextImporter()
        {
            this.dbH = new GeneralDbDataHelper(new UnitOfWork());
            dbH.Logger = new Mock<ILog>().Object;
            this.LocalManager = new LocalizationEditor(dbH);
        }

        public void ExtractFromExcel(string path)
        {
            ExcelQueryable<OrignTextTransModel> rows = this.GetMainWorkSheet(path);
            Console.WriteLine("Started Add/ Update");

            foreach (var item in rows)
            {
                string lastKnowFeature = item.FeatureValue;

                if (!string.IsNullOrWhiteSpace(item.FinTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.OriginalText,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.Finnish, TranslatedText = item.FinTextValue }
                    });
                }

                if (!string.IsNullOrWhiteSpace(item.EngTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.OriginalText,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.English, TranslatedText = item.EngTextValue }
                    });
                }

                if (!string.IsNullOrWhiteSpace(item.SweTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.OriginalText,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.Swedish, TranslatedText = item.SweTextValue }
                    });
                }
            }
        }

        public LinqToExcel.Query.ExcelQueryable<OrignTextTransModel> GetMainWorkSheet(string path)
        {
            excel = new ExcelQueryFactory(path);
            excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Jet;

            this.ValidateExcelColumns(excel);

            // Map columns
            excel.AddMapping<OrignTextTransModel>(m => m.OriginalText, Original);
            excel.AddMapping<OrignTextTransModel>(m => m.FeatureValue, Feature);
            excel.AddMapping<OrignTextTransModel>(m => m.EngTextValue, EnglishText);
            excel.AddMapping<OrignTextTransModel>(m => m.FinTextValue, FinnishText);
            excel.AddMapping<OrignTextTransModel>(m => m.SweTextValue, SwedishText);

            // Group by fature colu,m

            return excel.Worksheet<OrignTextTransModel>("Main");
        }

        private void AddUpdate(TranslatePageModel model)
        {
            var oringMain = new db.OriginalText
                {

                    Feature = model.Feature,
                    Original = model.Text,
                    OriginalTextTranslations = new List<db.OriginalTextTranslation>
                    {
                        new db.OriginalTextTranslation
                        {
                            Language = model.SelectedTranslatePageTranslationModel.Language.ToDbObject(),
                            Translation = model.SelectedTranslatePageTranslationModel.TranslatedText
                        }
                    }
                };

            //var origText = this.DbContext.Get<OriginalText>(o => o.Original == model.Text && o.Feature == model.Feature);
            var dborignText = this.dbH.Get<db.OriginalText>(o => o.Original == model.Text && o.Feature == model.Feature);

            if (dborignText != null)
            {
                foreach (var item in oringMain.OriginalTextTranslations)
                {
                    db.OriginalTextTranslation txtTRans = dborignText.OriginalTextTranslations.Where(o => o.Language == item.Language).FirstOrDefault();
                    item.OriginalTextId = dborignText.Id;

                    if (txtTRans == null)
                    {
                        this.dbH.Create<db.OriginalTextTranslation>(item);

                        Console.WriteLine("Create WebElementTranslation");
                        this.dbH.FlushChanges();
                        this.UpdateCounter++;
                    }
                    else
                    {
                        if (item.Translation.ToUpperInvariant().Trim() != txtTRans.Translation.ToUpperInvariant().Trim())
                        {
                            txtTRans.Translation = item.Translation;
                            this.dbH.Update<db.OriginalTextTranslation>(txtTRans);
                            Console.WriteLine("Update WebElementTranslation");
                            this.dbH.FlushChanges();
                            this.UpdateCounter++;
                        }
                    }

                }
            }
            else
            {
                this.dbH.Create<db.OriginalText>(oringMain);
                this.dbH.FlushChanges();
                Console.WriteLine("Create: {0}", model.Text);
            }
        }

        /// <summary>
        /// Check if necessary columns ir available
        /// </summary>
        /// <param name="input">ExcelQueryFactory instance</param>
        private void ValidateExcelColumns(ExcelQueryFactory input)
        {
            string notFound = "Not found";
            var excelcolumns = input.GetColumnNames("Main").ToList();

            List<string> OriginalColumnNames = new List<string> { "Feature", "Original", "English text", "Finish text", "Swedish text" };

            for (int i = 0; i < OriginalColumnNames.Count(); i++)
            {
                if (excelcolumns[i] != OriginalColumnNames[i])
                {
                    throw new ArgumentException("Column names have been changed or changed order!!!");
                }
            }

            if (excelcolumns.Count() == 0)
            {
                throw new ArgumentException("Main not found");
            }

            if (excelcolumns.Where(o => o == Feature).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", Feature, notFound));
            }

            if (excelcolumns.Where(o => o == FinnishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", FinnishText, notFound));
            }

            if (excelcolumns.Where(o => o == SwedishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", SwedishText, notFound));
            }

            if (excelcolumns.Where(o => o == EnglishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", EnglishText, notFound));
            }
        }

        public void DisposeReferences()
        {
            //this.excel.dis

        }

    }
}
