namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// VetumaBaseHelper class is used to share commom Vetuma imeplementation methods
    /// </summary>
    public class VetumaBaseHelper
    {
        /// <summary>
        /// VetumaTextModel object contains text translations
        /// </summary>
        protected VetumaTextModel TxtModel { get; set; }

        /// <summary>
        /// Default class constructor
        /// </summary>
        public VetumaBaseHelper()
        {
        }

        /// <summary>
        /// Overloaded class contructor 
        /// Translates text that are necessary for Vetuma auth/payment actions
        /// Use PROD and TEST translations cases -> Feature name: VetumaTextModel
        /// </summary>
        /// <param name="localManager">ILocalizationManager Instance</param>
        public VetumaBaseHelper(ILocalizationManager localManager)
        {
            if (localManager == null)
            {
                return;
            }

            this.TxtModel = new VetumaTextModel();
            string featureName = typeof(VetumaTextModel).Name;

#if !PROD
            this.TxtModel.PaymentDescription = localManager.GetTextTranslationTEST("PaymentDescription", featureName);
            this.TxtModel.MessageToSeller = localManager.GetTextTranslationTEST("MessageToSeller", featureName);
            this.TxtModel.VetumaButtonInstructions = localManager.GetTextTranslationTEST("VetumaButtonInstructions", featureName);
            this.TxtModel.VetumaButtonText = localManager.GetTextTranslationTEST("VetumaButtonText", featureName);
#else
            this.TxtModel.PaymentDescription = localManager.GetTextTranslationPROD("PaymentDescription", FeatureName);
            this.TxtModel.MessageToSeller = localManager.GetTextTranslationPROD("MessageToSeller", FeatureName);
            this.TxtModel.VetumaButtonInstructions = localManager.GetTextTranslationPROD("VetumaButtonInstructions", FeatureName);
            this.TxtModel.VetumaButtonText = localManager.GetTextTranslationPROD("VetumaButtonText", FeatureName);
#endif
        }
    }
}