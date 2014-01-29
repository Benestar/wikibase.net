using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Data value for globe coordinates
    /// </summary>
    public class GlobeCoordinateValue : DataValue
    {
        // TODO: Change globe to enum
        // TODO: Altitude as object?

        /// <summary>
        /// Full wikidata entity path for the Earth globe.
        /// </summary>
        public const String GLOBE_EARTH = "http://www.wikidata.org/entity/Q2";

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public Double latitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public Double longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        public Object altitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        public Double precision
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the globe on which the location resides.
        /// </summary>
        /// <value>The globe on which the location resides</value>
        public String globe
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="altitude">The altitude.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="globe">The globe on which the location resides.</param>
        public GlobeCoordinateValue(Double latitude, Double longitude, Object altitude, Double precision, String globe)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
            this.precision = precision;
            this.globe = globe;
        }

        internal GlobeCoordinateValue(JsonValue value)
        {
            JsonObject obj = value.asObject();
            this.latitude = obj.get("latitude").asDouble();
            this.longitude = obj.get("longitude").asDouble();
            this.altitude = obj.get("altitude");
            JsonValue precisionReceived = obj.get("precision");
            if ( precisionReceived != JsonValue.NULL )
            {
                this.precision = precisionReceived.asDouble();
            }
            this.globe = obj.get("globe").asString();
        }

        /// <summary>
        /// Gets the type identifier of the type at server side.
        /// </summary>
        /// <returns>The type identifier.</returns>
        public override String getType()
        {
            return "globecoordinate";
        }

        internal override JsonValue encode()
        {
            return new JsonObject()
                .add("latitude", latitude)
                .add("longitude", longitude)
                .add("altitude", altitude.ToString())
                .add("precision", precision)
                .add("globe", globe);
        }
    }
}