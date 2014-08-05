using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uma.Eservices.Common;
using Uma.Eservices.DbAccess;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing;
using System.IO;
using db = Uma.Eservices.DbObjects;
using System.Drawing;

namespace TranslationImporter
{
    public class TextExporter : IExtractLogic<OrignTextTransModel>
    {
        #region Properties

        public List<OrignTextTransModel> ExcelRows { get; set; }

        public List<OrignTextTransModel> SqlRows { get; set; }

        public GeneralDbDataHelper dbH { get; set; }

        public ExcelPackage Excel { get; set; }

        public ExcelWorksheet MainWorkSheet { get; set; }

        public ExcelWorksheet HistoryWorkSheet { get; set; }

        public TextImporter textImport { get; set; }

        public List<string> ExcelColumnNames { get; set; }

        public int UpdateCounter { get; set; }

        public int NewCounter { get; set; }

        #endregion

        public TextExporter()
        {
            this.dbH = new GeneralDbDataHelper(new UnitOfWork());
            dbH.Logger = new Mock<ILog>().Object;

            this.SqlRows = new List<OrignTextTransModel>();

            Console.WriteLine("Original Text exporter.");
        }

        public void ExtractFromDB(string path)
        {
            textImport = new TextImporter();
            // Header skipt via mapping
            this.ExcelRows = textImport.GetMainWorkSheet(path).ToList<OrignTextTransModel>();
            this.ExcelColumnNames = textImport.excel.GetColumnNames("Main").ToList();

            this.LoadExcel(path);
            this.PrepareExcel();
            this.MapSQLtoExcelModelList();
            this.StartWork();
            this.DisposeRederences();

            Console.WriteLine("Total updates " + this.UpdateCounter);
        }

        public void LoadExcel(string path)
        {
            this.Excel = new ExcelPackage(new FileInfo(path));
            Console.WriteLine("Load excel");
            this.MainWorkSheet = this.Excel.Workbook.Worksheets.Where(o => o.Name == "Main").FirstOrDefault();
            this.HistoryWorkSheet = this.Excel.Workbook.Worksheets.Where(o => o.Name == "Update history").FirstOrDefault();
            Console.WriteLine("Load Main - History workSheets");
            if (this.MainWorkSheet == null || this.HistoryWorkSheet == null)
            {
                this.Excel.Dispose();
                throw new ArgumentException("MainWorkSheet or HIstory Sheet is null");
            }
        }

        public void PrepareExcel()
        {
            Console.WriteLine("Set default colors");
            // Set main workSheet row bck colors back to default
            for (int i = 2; i < this.ExcelRows.Count(); i++)
            {
                var row = this.MainWorkSheet.Row(i);
                row.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            }
        }

        public void MapSQLtoExcelModelList()
        {
            Console.WriteLine("Get all columns from DB -> maps to model");
            // method maps DB rows to List<ExcelModel
            var orginList = this.dbH.GetAll<db.OriginalText>().ToList();

            foreach (var item in orginList)
            {
                var tempEngValue = item.OriginalTextTranslations.Where(o => o.Language == db.SupportedLanguage.English).FirstOrDefault();
                var tempFinValue = item.OriginalTextTranslations.Where(o => o.Language == db.SupportedLanguage.Finnish).FirstOrDefault();
                var tempSweValue = item.OriginalTextTranslations.Where(o => o.Language == db.SupportedLanguage.Swedish).FirstOrDefault();

                this.SqlRows.Add(new OrignTextTransModel
                {
                    OriginalText = item.Original,
                    FeatureValue = item.Feature,
                    EngTextValue = tempEngValue != null ? tempEngValue.Translation : null,
                    FinTextValue = tempFinValue != null ? tempFinValue.Translation : null,
                    SweTextValue = tempSweValue != null ? tempSweValue.Translation : null
                });
            }
        }

        public void StartWork()
        {
            Console.WriteLine("Start comparing rows");
            // of row in existing tranlsatin is not found then it should be added to excel and colored green
            foreach (var item in this.SqlRows)
            {
                // compare SQL row with excel row add updates if necessary
                //  If update needed colorize row -> GREEN
                if (this.CompareUpdate(item))
                {
                    Console.WriteLine("New row insert");
                    this.InserNewRows(item);
                }
            }

            // Insert History row at the start
            this.HistoryWorkSheet.InsertRow(2, 1);
            this.HistoryWorkSheet.SetValue(2, 1, DateTime.Now.ToString());
            this.HistoryWorkSheet.SetValue(2, 2, this.NewCounter);
            this.HistoryWorkSheet.SetValue(2, 3, this.UpdateCounter);
            // this.HistoryWorkSheet.
        }

        public void InserNewRows(OrignTextTransModel item)
        {
            // Insert row at start

            this.MainWorkSheet.InsertRow(2, 1);
            var row = this.MainWorkSheet.Row(2);

            // set sell values for new row - already validated row order.
            SetCellValueColorBlue(this.MainWorkSheet.Cells[2, 1], item.FeatureValue);
            SetCellValueColorBlue(this.MainWorkSheet.Cells[2, 2], item.OriginalText);

            SetCellValueColorBlue(this.MainWorkSheet.Cells[2, 3], item.EngTextValue);
            SetCellValueColorBlue(this.MainWorkSheet.Cells[2, 4], item.FinTextValue);
            SetCellValueColorBlue(this.MainWorkSheet.Cells[2, 5], item.SweTextValue);

            this.NewCounter++;
        }

        private void SetCellValueColorBlue(ExcelRange cell, string textValue)
        {
            cell.Value = textValue;
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.Blue);
        }

        public bool CompareUpdate(OrignTextTransModel item)
        {
            // returns true  if n0 match is found

            // compare rows -> by feature -> orign text
            var row = this.ExcelRows.Where(o => o.FeatureValue == item.FeatureValue && o.OriginalText == item.OriginalText).FirstOrDefault();

            // if row not found return true
            if (row == null)
            {
                return true;
            }

            this.CompareUpdateTranslation(row, item);
            return false;
        }

        public void CompareUpdateTranslation(OrignTextTransModel Excelrow, OrignTextTransModel Sqlrow)
        {
            // refactor to speed up
            int ExcelRowIndex = this.ExcelRows.FindIndex(o => o.FeatureValue == Excelrow.FeatureValue && o.OriginalText == Excelrow.OriginalText) + 2;

            // if row is found compare translations
            // if translation is difernt update excel row from sql, colorize background
            if (Excelrow.EngTextValue != Sqlrow.EngTextValue)
            {
                var cell = this.MainWorkSheet.Cells[ExcelRowIndex, 3];
                this.SetCellValueColorGreen(cell, Sqlrow.EngTextValue);
            }

            if (Excelrow.FinTextValue != Sqlrow.FinTextValue)
            {
                var cell = this.MainWorkSheet.Cells[ExcelRowIndex, 4];
                this.SetCellValueColorGreen(cell, Sqlrow.FinTextValue);
            }

            if (Excelrow.SweTextValue != Sqlrow.SweTextValue)
            {
                var cell = this.MainWorkSheet.Cells[ExcelRowIndex, 5];
                this.SetCellValueColorGreen(cell, Sqlrow.SweTextValue);
            }
        }

        private void SetCellValueColorGreen(ExcelRange cell, string textValue)
        {
            cell.Value = textValue;

            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.LawnGreen);
            this.UpdateCounter++;
        }

        public void DisposeRederences()
        {
            //save and dispose

            this.Excel.Save();

            this.Excel.Dispose();
        }
    }
}
