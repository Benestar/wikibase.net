using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Possible values for the <see cref="GlobeCoordinateValue.Globe"/>.
    /// </summary>
    public enum Globe
    {
        /// <summary>
        /// Unknown globe value.
        /// </summary>
        Unknown,

        /// <summary>
        /// Earth.
        /// </summary>
        Earth,
    }

    /// <summary>
    /// Data value for globe coordinates
    /// </summary>
    public class GlobeCoordinateValue : DataValue
    {
        #region Json names

        /// <summary>
        /// The identifier of this data type in the serialized json object.
        /// </summary>
        public const String TypeJsonName = "globecoordinate";

        /// <summary>
        /// The name of the <see cref="Latitude"/> property in the serialized json object.
        /// </summary>
        private const String LatitudeJsonName = "latitude";

        /// <summary>
        /// The name of the <see cref="Longitude"/> property in the serialized json object.
        /// </summary>
        private const String LongitudeJsonName = "longitude";

        /// <summary>
        /// The name of the deprecated altitude property in the serialized json object.
        /// </summary>
        private const String AltitudeJsonName = "altitude";

        /// <summary>
        /// The name of the <see cref="Precision"/> property in the serialized json object.
        /// </summary>
        private const String PrecisionJsonName = "precision";

        /// <summary>
        /// The name of the <see cref="Globe"/> property in the serialized json object.
        /// </summary>
        private const String GlobeJsonName = "globe";

        #endregion Json names

        private Dictionary<Globe, String> _globeJsonNames = new Dictionary<Globe, String>()
        {
             {Globe.Earth, "http://www.wikidata.org/entity/Q2" }
        };

        // TODO: Change globe to enum
        // TODO: Altitude as object?

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public Double Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public Double Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        public Double Precision
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the globe on which the location resides.
        /// </summary>
        /// <value>The globe on which the location resides.</value>
        public Globe Globe
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="globe">The globe on which the location resides.</param>
        public GlobeCoordinateValue(Double latitude, Double longitude, Double precision, Globe globe)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Precision = precision;
            this.Globe = globe;
        }

        /// <summary>
        /// Parses a <see cref="JsonValue"/> to a globe coordinate value.
        /// </summary>
        /// <param name="value"><see cref="JsonValue"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        internal GlobeCoordinateValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            JsonObject obj = value.asObject();
            this.Latitude = obj.get(LatitudeJsonName).asDouble();
            this.Longitude = obj.get(LongitudeJsonName).asDouble();
            var altitude = obj.get(AltitudeJsonName);  // deprecated
            JsonValue precisionReceived = obj.get(PrecisionJsonName);
            if ( precisionReceived != JsonValue.NULL )
            {
                this.Precision = precisionReceived.asDouble();
            }
            var globe = obj.get(GlobeJsonName).asString();
            if ( _globeJsonNames.Any(x => x.Value == globe) )
            {
                this.Globe = _globeJsonNames.First(x => x.Value == globe).Key;
            }
            else
            {
                this.Globe = Globe.Unknown;
            }
        }

        /// <summary>
        /// Gets the type identifier of the type at server side.
        /// </summary>
        /// <returns>The type identifier.</returns>
        protected override String JsonName()
        {
            return TypeJsonName;
        }

        /// <summary>
        /// Encodes as a <see cref="JsonValue"/>.
        /// </summary>
        /// <returns>Encoded class.</returns>
        /// <exception cref="InvalidOperationException"><see cref="GlobeCoordinateValue.Globe"/> is <see cref="Wikibase.DataValues.Globe.Unknown"/>.</exception>
        internal override JsonValue Encode()
        {
            if ( Globe == Globe.Unknown )
            {
                throw new InvalidOperationException("Globe value not set.");
            }

            return new JsonObject()
                .add(LatitudeJsonName, Latitude)
                .add(LongitudeJsonName, Longitude)
                // .add(AltitudeJsonName, altitude.ToString())
                .add(PrecisionJsonName, Precision)
                .add(GlobeJsonName, _globeJsonNames[Globe]);
        }
    }
}