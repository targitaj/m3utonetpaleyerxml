using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationImporter
{
    public class WebElementExcelModel
    {
        public WebElementExcelModel()
        {
            // Incomeny in case on LinqToExcel updaes

            //this.Prompt = new TextTransLang();
            //this.ControlText = new TextTransLang();
            //this.Help = new TextTransLang();
            //this.EnumText = new TextTransLang();
        }

        public string ModelName { get; set; }

        public string PropertyName { get; set; }

        #region  Need for Linq to Excel update

        //public TextTransLang Prompt { get; set; }

        //public TextTransLang ControlText { get; set; }

        //public TextTransLang Help { get; set; }

        //public TextTransLang EnumText { get; set; } 

        #endregion

        #region map properties

        // ====== Label
        public string LabelEngText { get; set; }

        public string LabelFinText { get; set; }

        public string LabelSweText { get; set; }

        // ====== Help
        public string HelpEngText { get; set; }

        public string HelpFinText { get; set; }

        public string HelpSweText { get; set; }

        public string ControlEngText { get; set; }

        public string ControlFinText { get; set; }

        public string ControlSweText { get; set; }

        // ====== Enum
        public string EnumEngText { get; set; }

        public string EnumFinText { get; set; }

        public string EnumSweText { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("Model: {0} Prop: {1}", this.ModelName, this.PropertyName);
        }

    }

    public class TextTransLang
    {
        public TextTransLang()
        {
            this.EngText = "";
            this.FinText = "";
            this.SweText = "";
        }

        public string EngText { get; set; }

        public string FinText { get; set; }

        public string SweText { get; set; }
    }
}
