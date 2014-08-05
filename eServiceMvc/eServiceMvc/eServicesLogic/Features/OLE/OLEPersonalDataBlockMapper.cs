namespace Uma.Eservices.Logic.Features.OLE
{
    using db = Uma.Eservices.DbObjects.OLE;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;

    public static class OLEPersonalDataBlockMapper
    {
        #region From Db obj to Web Model obj

        public static OLEPersonalDataBlock ToWebModel(this db.OLEPersonalDataBlock input)
        {
            return new OLEPersonalDataBlock
            {
                BirthCountry = input.BirthCountry,
                Birthday = input.Birthday,
                BirthPlace = input.BirthPlace,
                CommunicationLanguage = input.CommunicationLanguage.ToWebModel(),
                CurrentCitizenships = input.CurrentCitizenships.ToWebModel(),
                Education = input.Education,
                Gender = input.Gender.ToWebModel(),
                MotherLanguage = input.MotherLanguage,
                Occupation = input.Occupation,
                PersonCode = input.PersonCode,
                PersonName = input.PersonName.ToWebModel(),
                PreviousCitizenships = input.PreviousCitizenships.ToWebModel(),
                PreviousNames = input.PreviousNames.ToWebModel()
            };
        }


        #endregion

        #region From Web Model obj to Db obj

        #endregion
    }
}
