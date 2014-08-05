using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db = Uma.Eservices.DbObjects.OLE;
using Uma.Eservices.Models.OLE;
using Uma.Eservices.Logic.Features.FormCommonsMapper;

namespace Uma.Eservices.Logic.Features.OLE
{
    public static class ContactInformationBlockMapper
    {
        #region From Db obj to Web Model obj

        public static OLEContactInfoBlock ToWebModel(this db.OLEContactInfoBlock input)
        {
            return new OLEContactInfoBlock
            {
                AddressInformation = input.AddressInformation.ToWebModel(),
                EmailAddress = input.EmailAddress,
                FinlandAddressInformation = input.FinlandAddressInformation.ToWebModel(),
                FinlandEmailAddress = input.FinlandEmailAddress,
                FinlandTelephoneNumber = input.FinlandTelephoneNumber,
                TelephoneNumber = input.TelephoneNumber
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        #endregion
    }
}
