namespace Uma.Eservices.Logic.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Uma.Eservices.Models.Sandbox;
    using Uma.Eservices.UmaConnClient;

    /// <summary>
    /// Class for Business Logic that may be required to have in Sandbox
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SandboxLogic : ISandboxLogic
    {
        /// <summary>
        /// The holder for database helper
        /// </summary>
        // private readonly IGeneralDataHelper databaseHelper;

        /// <summary>
        /// Holder of Uma Connector - WCF service proxy
        /// </summary>
//        private readonly IUmaConnProxy<IUmaMasterDataService> umaData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxLogic" /> class.
        /// </summary>
        /// <param name="umaData">Uma Connector - WCF service proxy</param>
//        public SandboxLogic(IUmaConnProxy<IUmaMasterDataService> umaData)
//        {
//            this.umaData = umaData;
//        }

        /// <summary>
        /// Test Dictionary filled with States  Key / Value
        /// </summary>
        private Dictionary<string, string> StateList
        {
            get
            {
                // First try to get countries from UMA
                //var umaCountries = this.umaData.Execute(uma => uma.GetCountries());
                //if (umaCountries != null && umaCountries.OperationCallStatus == CallStatus.Success && umaCountries.Countries != null && umaCountries.Countries.Count > 0)
               // {
               //     return umaCountries.Countries.OrderBy(c => c.NameEnglish).ToDictionary(masterDataCountry => masterDataCountry.Label, masterDataCountry => masterDataCountry.NameEnglish);
               // }

                // If UMA call failed - supply hardcoded
                return new Dictionary<string, string>
                {
                    { "ACCT_21", "UMA call failed" }, { "AFGANISTAN_13", "Afghanistan" }, { "ALBANIA_16", "Albania" }, { "ITAVALTA_81", "Austria" }, { "AZERBAIDZHAN_22", "Azerbaijan" },
                    { "CHILE_41", "Chile" }, { "KIINA_96", "China" }, { "KOLUMBIA_99", "Colombia" }, { "KOMORIT_100", "Comoros" }, { "KUUBA_107", "Cuba" }, 
                    { "KYPROS_112", "Cyprus" }, { "TSEKKI_202", "Czech Republic" }, { "KONGON_DEMOKRAATTINEN_TA_", "Congo, the Democratic Republic of the" },
                    { "KOLUMBIA_", "Colombia" }, { "KOMORIT_", "Comoros" }, { "KAMPUTSEA_", "Kampuchea" }, { "KREIKKA_", "Greece" },
                    { "KIRGISIA_", "Kyrgyzstan" }, { "KROATIA_", "Croatia" }, { "KAMERUN_", "Cameroon" }, { "KOREAN_TASAVALTA_", "Korea, Republic of" },
                    { "KUUBA_", "Cuba" }, { "KAP_VERDE_", "Cape Verde" }, { "KANSALAISUUDETON_", "Stateless" }, { "KUWAIT_", "Kuwait" },
                    { "KYPROS_", "Cyprus" }, { "LAOS_", "Lao People`s Democratic Republic" }, { "LATVIA_", "Latvia" }, { "LUXEMBURG_", "Luxembourg" },
                    { "LIBYA_", "Libyan Arab Jamahiriya" }, { "LIECHTENSTEIN_", "Liechtenstein" }, { "LESOTHO_", "Lesotho" }, { "LIBANON_", "Lebanon" },
                    { "LIETTUA_", "Lithuania" }, { "LIBERIA_", "Liberia" }, { "LANSISAMOA_", "Samoa" }, { "MACAO_", "Macau" },
                    { "MAKEDONIA_", "Macedonia, The former Yugoslav Republic of" }, { "MALI_", "Mali" }, { "MOSAMBIK_", "Mozambique" }, { "MEKSIKO_", "Mexico" },
                    { "MADAGASKAR_", "Madagascar" }, { "MIKRONESIA_", "Micronesia, Federated  States of" }, { "MALESIA_", "Malaysia" }, { "MALTA_", "Malta" },
                    { "MONGOLIA_", "Mongolia" }, { "MOLDOVA_", "Moldova, Republic of" }, { "MONACO_", "Monaco" }, { "MAURITANIA_", "Mauritania" },
                    { "MAROKKO_", "Morocco" }, { "MAURITIUS_", "Mauritius" }, { "MALEDIIVIT_", "Maldives" }, { "MALAWI_", "Malawi" },
                    { "MYANMAR_", "Myanmar" }, { "NAMIBIA_", "Namibia" }, { "NAURU_", "Nauru" }, { "NICARAGUA_", "Nicaragua" },
                    { "NEPAL_", "Nepal" }, { "NIGER_", "Niger" }, { "NIGERIA_", "Nigeria" }, { "NEUVOSTOLIITTO_", "Soviet Union" }
                };
            }
        }

        /// <summary>
        /// Gets the single column form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        public TestFormModel GetTestFormModel(int? id)
        {
            // var alphas = this.databaseHelper.Count<ApplicationUser>(t => t.UserName.StartsWith("A"));
            // var umas = this.umaData.Execute(s => s.GetCodeByLabel("RUSSIA"));
            var model = new TestFormModel();
            model.CountryList = this.StateList;
            if (!id.HasValue)
            {
                // No ID - just empty model (as to create new object)
                // model.FirstField = "UMA WCF: " + umas.Code.Label + " = " + umas.Code.TextFinnish;
                return model;
            }

            // Assume we are calling here DB or service logic and getting object which map into model
            model.FirstField = "Dummy data from logic (faking DB/Svc load)";
            model.RequiredField = "Something for your requirements";
            model.ValidationField = "123, the validation";
            return model;
        }

        /// <summary>
        /// Saves the single column layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="model">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        public int SaveTestForm(TestFormModel model)
        {
            // here we would call some external persistence object method to save model data
            // and return this object ID (just in case, if it needs to be loaded by READ/GET screen)
            return 21;
        }

        /// <summary>
        /// Saves the KAN7Model layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="toggleSwichModel">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        public int SavePdfSampleModel(Kan7Model toggleSwichModel)
        {
            return 69;
        }

        /// <summary>
        /// Gets the PdfSampleModel form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        public Kan7Model GetPdfSampleModel(int? id)
        {
            Kan7Model k7 = new Kan7Model()
            {
                Description = "This is Desc for KAN_7",
                Title = "KAN_7  mega pdf form",
                // TypeOfApplication = "Type of apppp",
                StateList = this.StateList,
                Applicationid = 87
            };

            SpouseModel spmodel = new SpouseModel()
            {
                Header = "This is Default Spouse model header",
                BirthPlace = "Liibija",
                DateOfBirth = new DateTime(1655, 11, 11),
                FirstNames = "Pirmais vards",
                LastName = "Uzvardsss",
                IdNumber = "ID-2345675432",
                SpouseStateOfBirth = "US and A"
            };
            spmodel.FormerCitiz.Add(new GroupSpouseFormerCitiz() { Citiz = "Citizer1", CitizHowGotten = "Nezinu How", CitizWhenGotten = new DateTime(2012, 12, 12) });
            spmodel.FormerCitiz.Add(new GroupSpouseFormerCitiz() { Citiz = "Citizer123424", CitizHowGotten = "Nezinu Howeeeeeeeeee", CitizWhenGotten = new DateTime(2011, 3, 11) });
            spmodel.FormerCitiz.Add(new GroupSpouseFormerCitiz() { Citiz = "Citizer1234234", CitizHowGotten = "Nezinu Howiiiiiiiii", CitizWhenGotten = new DateTime(2010, 2, 22) });

            spmodel.FormerNames.Add(new GroupSpouseFormerNames() { FirstNames = "F_name", LastName = "L_Name", IsHot = true });
            spmodel.FormerNames.Add(new GroupSpouseFormerNames() { FirstNames = "F_namesssss", LastName = "L_Namennnnn" });
            spmodel.FormerNames.Add(new GroupSpouseFormerNames() { FirstNames = "F_nameuuuuuuu", LastName = "L_NameBBBBBBBB", IsHot = true });

            k7.SpouseModel = spmodel;

            return k7;
        }
    }
}
