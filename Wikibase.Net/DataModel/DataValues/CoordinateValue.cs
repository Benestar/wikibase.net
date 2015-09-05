using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class CoordinateValue : DataValue
    {
        public const string GLOBE_EARTH = "https://www.wikidata.org/entity/Q2";

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Globe { get; set; }

        public CoordinateValue(double longitude, double latitude, string globe)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Globe = globe;
        }
    }
}
