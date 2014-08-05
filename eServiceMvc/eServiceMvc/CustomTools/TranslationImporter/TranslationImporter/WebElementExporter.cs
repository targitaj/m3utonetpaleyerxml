using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uma.Eservices.DbAccess;
using Uma.Eservices.Common;
using Uma.Eservices.DbObjects;
using Uma.Eservices.Logic.Features.Localization;
using Moq;
using System.IO;
using System.Drawing;

namespace TranslationImporter
{
    public class WebElementExporter : IExtractLogic<WebElementExcelModel>
    {
        #region properties
        public List<WebElementExcelModel> ExcelRows { get; set; }

        public List<WebElementExcelModel> SqlRows { get; set; }

        public GeneralDbDataHelper dbH { get; set; }

        public ExcelPackage Excel { get; set; }

        public ExcelWorksheet MainWorkSheet { get; set; }

        public ExcelWorksheet HistoryWorkSheet { get; set; }

        public WebElementImporter WebElemImporter { get; set; }

        public List<string> ExcelColumnNames { get; set; }

        public int UpdateCounter { get; set; }

        public int NewCounter { get; set; }

        #endregion

        public WebElementExporter()
        {
            this.dbH = new GeneralDbDataHelper(new UnitOfWork());
            dbH.Logger = new Mock<ILog>().Object;

            this.SqlRows = new List<WebElementExcelModel>();

            Console.WriteLine("Web Element translation exporter.");
        }


        public void ExtractFromDB(string path)
        {
            this.WebElemImporter = new WebElementImporter();
            this.ExcelRows = this.WebElemImporter.GetMainWorkSheet(path).ToList<WebElementExcelModel>();
            this.ExcelColumnNames = this.WebElemImporter.excel.GetColumnNames("Main").ToList();

            this.LoadExcel(path);
            this.PrepareExcel();
            this.MapSQLtoExcelModelList();
            this.StartWork();
            this.DisposeRederences();

            Console.WriteLine("Updated: " + this.UpdateCounter);
            Console.WriteLine("New: " + this.NewCounter);

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
            var webElemList = this.dbH.GetAll<WebElement>().ToList();

            foreach (var item in webElemList)
            {
                // map modelname, propertyname
                WebElementExcelModel model = new WebElementExcelModel();
                model.PropertyName = item.PropertyName;
                model.ModelName = item.ModelName;

                // map label
                this.MapTranslationTypes(model, item, TranslatedTextType.Label);
                this.MapTranslationTypes(model, item, TranslatedTextType.Help);
                this.MapTranslationTypes(model, item, TranslatedTextType.ControlText);
                this.MapTranslationTypes(model, item, TranslatedTextType.EnumText);

                this.SqlRows.Add(model);
            }
            Console.WriteLine("Mappind ended totatl: " + this.SqlRows.Count());
        }

        private void MapTranslationTypes(WebElementExcelModel model, WebElement item, TranslatedTextType type)
        {
            var typeLabel = item.WebElementTranslations.Where(o => o.TranslationType == type).ToList();

            var engLabelTr = typeLabel.Where(o => o.Language == SupportedLanguage.English).FirstOrDefault();
            var finLabelTr = typeLabel.Where(o => o.Language == SupportedLanguage.Finnish).FirstOrDefault();
            var sweLabelTr = typeLabel.Where(o => o.Language == SupportedLanguage.Swedish).FirstOrDefault();

            switch (type)
            {
                case TranslatedTextType.ControlText:
                    model.ControlEngText = engLabelTr == null ? null : engLabelTr.TranslatedText;
                    model.ControlFinText = finLabelTr == null ? null : finLabelTr.TranslatedText;
                    model.ControlSweText = sweLabelTr == null ? null : sweLabelTr.TranslatedText;
                    break;
                case TranslatedTextType.EnumText:
                    model.EnumEngText = engLabelTr == null ? null : engLabelTr.TranslatedText;
                    model.EnumFinText = finLabelTr == null ? null : finLabelTr.TranslatedText;
                    model.EnumSweText = sweLabelTr == null ? null : sweLabelTr.TranslatedText;
                    break;
                case TranslatedTextType.Help:
                    model.HelpEngText = engLabelTr == null ? null : engLabelTr.TranslatedText;
                    model.HelpFinText = finLabelTr == null ? null : finLabelTr.TranslatedText;
                    model.HelpSweText = sweLabelTr == null ? null : sweLabelTr.TranslatedText;
                    break;
                case TranslatedTextType.Label:
                    model.LabelEngText = engLabelTr == null ? null : engLabelTr.TranslatedText;
                    model.LabelFinText = finLabelTr == null ? null : finLabelTr.TranslatedText;
                    model.LabelSweText = sweLabelTr == null ? null : sweLabelTr.TranslatedText;
                    break;
                default:
                    break;
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

        public void InserNewRows(WebElementExcelModel item)
        {
            // Insert row at start
            this.MainWorkSheet.InsertRow(3, 1);
            var row = this.MainWorkSheet.Row(3);
            row.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            row.Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);

            // set sell values for new row - already validated row order.
            this.MainWorkSheet.Cells[3, 1].Value = item.ModelName;
            this.MainWorkSheet.Cells[3, 2].Value = item.PropertyName;

            // labe
            this.SetLabelCellValues(item.LabelEngText, item.LabelFinText, item.LabelSweText, 3);

            //contorltext
            this.SetControlTextCellValues(item.ControlEngText, item.ControlFinText, item.ControlSweText, 3);

            // help
            this.SetHelpCellValues(item.HelpEngText, item.HelpFinText, item.HelpSweText, 3);

            // enum
            this.SetEnumCellValues(item.EnumEngText, item.EnumFinText, item.EnumSweText, 3);

            this.NewCounter++;
        }

        #region  Set Cell value by trans type
        private void SetLabelCellValues(string eng, string fin, string swe, int rowIndex)
        {
            // label
            this.MainWorkSheet.Cells[rowIndex, 3].Value = eng;
            this.MainWorkSheet.Cells[rowIndex, 4].Value = fin;
            this.MainWorkSheet.Cells[rowIndex, 5].Value = swe;
        }

        private void SetControlTextCellValues(string eng, string fin, string swe, int rowIndex)
        {
            // contorl text
            this.MainWorkSheet.Cells[rowIndex, 6].Value = eng;
            this.MainWorkSheet.Cells[rowIndex, 7].Value = fin;
            this.MainWorkSheet.Cells[rowIndex, 8].Value = swe;
        }

        private void SetHelpCellValues(string eng, string fin, string swe, int rowIndex)
        {
            // help
            this.MainWorkSheet.Cells[rowIndex, 9].Value = eng;
            this.MainWorkSheet.Cells[rowIndex, 10].Value = fin;
            this.MainWorkSheet.Cells[rowIndex, 11].Value = swe;
        }

        private void SetEnumCellValues(string eng, string fin, string swe, int rowIndex)
        {
            // enum text
            this.MainWorkSheet.Cells[rowIndex, 12].Value = eng;
            this.MainWorkSheet.Cells[rowIndex, 13].Value = fin;
            this.MainWorkSheet.Cells[rowIndex, 14].Value = swe;
        }

        #endregion

        public bool CompareUpdate(WebElementExcelModel item)
        {
            // returns true  if n0 match is found

            // compare rows -> by feature -> orign text
            var row = this.ExcelRows.Where(o => o.ModelName == item.ModelName & o.PropertyName == item.PropertyName).FirstOrDefault();

            // if row not found return true
            if (row == null)
            {
                return true;
            }

            this.CompareUpdateTranslation(row, item);
            return false;
        }

        public void CompareUpdateTranslation(WebElementExcelModel Excelrow, WebElementExcelModel Sqlrow)
        {
            // refactor to speed up
            int ExcelRowIndex = this.ExcelRows.FindIndex(o => o.ModelName == Excelrow.ModelName & o.PropertyName == Excelrow.PropertyName) + 2;

            // if row is found compare translations
            // if translation is difernt update excel row from sql, colorize background

            this.CompareUpdateLabelType(Excelrow, Sqlrow, ExcelRowIndex);
            this.CompareUpdateHelpType(Excelrow, Sqlrow, ExcelRowIndex);
            this.CompareUpdateControlType(Excelrow, Sqlrow, ExcelRowIndex);
            this.CompareUpdateEnumType(Excelrow, Sqlrow, ExcelRowIndex);
        }

        #region Compare update cell by trans type

        private void CompareUpdateLabelType(WebElementExcelModel Excelrow, WebElementExcelModel Sqlrow, int rowIndex)
        {
            if (Excelrow.LabelEngText != Sqlrow.LabelEngText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 3], Sqlrow.LabelEngText);
            }

            if (Excelrow.LabelFinText != Sqlrow.LabelFinText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 4], Sqlrow.LabelEngText);
            }

            if (Excelrow.LabelSweText != Sqlrow.LabelSweText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 5], Sqlrow.LabelEngText);
            }
        }

        private void CompareUpdateHelpType(WebElementExcelModel Excelrow, WebElementExcelModel Sqlrow, int rowIndex)
        {
            if (Excelrow.HelpEngText != Sqlrow.HelpEngText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 6], Sqlrow.HelpEngText);
            }

            if (Excelrow.HelpFinText != Sqlrow.HelpFinText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 7], Sqlrow.HelpFinText);
            }

            if (Excelrow.HelpSweText != Sqlrow.HelpSweText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 8], Sqlrow.HelpSweText);
            }
        }

        private void CompareUpdateControlType(WebElementExcelModel Excelrow, WebElementExcelModel Sqlrow, int rowIndex)
        {
            if (Excelrow.ControlEngText != Sqlrow.ControlEngText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 9], Sqlrow.ControlEngText);
            }

            if (Excelrow.ControlFinText != Sqlrow.ControlFinText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 10], Sqlrow.ControlFinText);
            }

            if (Excelrow.ControlSweText != Sqlrow.ControlSweText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 11], Sqlrow.ControlSweText);
            }
        }

        private void CompareUpdateEnumType(WebElementExcelModel Excelrow, WebElementExcelModel Sqlrow, int rowIndex)
        {
            if (Excelrow.EnumEngText != Sqlrow.EnumEngText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 12], Sqlrow.EnumEngText);
            }

            if (Excelrow.EnumFinText != Sqlrow.EnumFinText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 13], Sqlrow.EnumFinText);
            }

            if (Excelrow.EnumSweText != Sqlrow.EnumSweText)
            {
                SetCellValueColorGreen(this.MainWorkSheet.Cells[rowIndex, 14], Sqlrow.EnumSweText);
            }
        }

        #endregion

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
