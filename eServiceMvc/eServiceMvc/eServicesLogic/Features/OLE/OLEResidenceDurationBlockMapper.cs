using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uma.Eservices.Models.OLE;
using db = Uma.Eservices.DbObjects.OLE;

namespace Uma.Eservices.Logic.Features.OLE
{
    public static class OLEResidenceDurationBlockMapper
    {
        #region From Db obj to Web Model obj

        public static OLEResidenceDurationBlock ToWebModel(this db.OLEResidenceDurationBlock input)
        {
            return new OLEResidenceDurationBlock
            {
                AlreadyInFinland = input.AlreadyInFinland,
                ArrivalDate = input.ArrivalDate,
                DepartDate = input.DepartDate,
                DurationOfStay = input.DurationOfStay
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        #endregion

    }
}
