namespace TranslationImporter
{
    using System;

    public class OrignTextTransModel
    {
        public string OrignTextId { get; set; }
        public string OriginalText { get; set; }
        public string FeatureValue { get; set; }
        public string EngTextValue { get; set; }
        public string FinTextValue { get; set; }
        public string SweTextValue { get; set; }

        public override string ToString()
        {
            return string.Format("F: {0}  Orign: {1}", this.FeatureValue, this.OriginalText);
        }

    }

    public enum OrignTextRowIndexes
    {
        // WARNING: should be same order as excel
        OrignTextId = 1,
        FeatureValue,
        OriginalText,
        EngTextValue,
        FinTextValue,
        SweTextValue
    }
}
