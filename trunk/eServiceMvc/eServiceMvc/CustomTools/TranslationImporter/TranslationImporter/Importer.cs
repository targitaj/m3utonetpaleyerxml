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
using Uma.Eservices.Logic.Features.Localization;
using Uma.Eservices.Models.Localization;

namespace TranslationImporter
{
    public class Importer
    {
        public ILocalizationEditor LocalManager { get; set; }

        public string Feature { get { return "Feature (Tech term)"; } }
        public string EnglishText { get { return "English text"; } }
        public string FinnishText { get { return "Finish Text "; } }
        public string SwedishText { get { return "Swedish text"; } }

        public Importer()
        {
            GeneralDbDataHelper dbH = new GeneralDbDataHelper(new UnitOfWork());
            dbH.Logger = new Mock<ILog>().Object;
            this.LocalManager = new LocalizationEditor(dbH);
        }

        public void ExtractFromExcel(string path)
        {
            var excel = new ExcelQueryFactory(path);
            excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Jet;

            this.ValidateExcelColumns(excel);

            // Map columns
            excel.AddMapping<ExcelModel>(m => m.FeatureValue, Feature);
            excel.AddMapping<ExcelModel>(m => m.EngTextValue, EnglishText);
            excel.AddMapping<ExcelModel>(m => m.FinTextValue, FinnishText);
            excel.AddMapping<ExcelModel>(m => m.SweTextValue, SwedishText);

            // Group by fature colu,m
            var rows = excel.Worksheet<ExcelModel>(0);
            string lastKnowFeature = string.Empty;

            foreach (var item in rows)
            {
                lastKnowFeature = lastKnowFeature != item.FeatureValue &&
                                  item.FeatureValue != null ?
                                  item.FeatureValue : lastKnowFeature;

                if (string.IsNullOrWhiteSpace(item.EngTextValue))
                {
                    // In some cases there are not provided Eng translation 
                    // but ar something not valid in other languages
                    // e.g.  delete
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(item.FinTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.EngTextValue,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.Finnish, TranslatedText = item.FinTextValue }
                    });
                }

                if (!string.IsNullOrWhiteSpace(item.EngTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.EngTextValue,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.English, TranslatedText = item.EngTextValue }
                    });
                }

                if (!string.IsNullOrWhiteSpace(item.SweTextValue))
                {
                    AddUpdate(new TranslatePageModel
                    {
                        Feature = lastKnowFeature,
                        Text = item.SweTextValue,
                        SelectedTranslatePageTranslationModel = new TranslatePageTranslationModel { Language = SupportedLanguage.Swedish, TranslatedText = item.SweTextValue }
                    });
                }

                if (!string.IsNullOrWhiteSpace(item.EngTextValue))
                    Console.WriteLine("F: {0}  VAL: {1}", lastKnowFeature, item.EngTextValue);
            }

        }

        private void AddUpdate(TranslatePageModel model)
        {
            this.LocalManager.AddUpdateOriginalText(model);
        }

        /// <summary>
        /// Check if necessary columns ir available
        /// </summary>
        /// <param name="input">ExcelQueryFactory instance</param>
        private void ValidateExcelColumns(ExcelQueryFactory input)
        {
            string notFound = "Not found";
            var columnNames = input.GetColumnNames("Sheet1");

            if (columnNames.Count() == 0)
            {
                throw new ArgumentException("Sheet1 not found");
            }

            if (columnNames.Where(o => o == Feature).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", Feature, notFound));
            }

            if (columnNames.Where(o => o == FinnishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", FinnishText, notFound));
            }

            if (columnNames.Where(o => o == SwedishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", SwedishText, notFound));
            }

            if (columnNames.Where(o => o == EnglishText).Count() == 0)
            {
                throw new ArgumentException(string.Format("{0}: {1}", EnglishText, notFound));
            }
        }

    }
}
