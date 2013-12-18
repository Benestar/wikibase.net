using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;
using Wikibase.DataValues;

namespace Wikibase
{
    /// <summary>
    /// A snak, a property with its value.
    /// </summary>
    public class Snak
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        /// <remarks>Allowed values are "value", "none" and "some". However, only "value" is used.</remarks>
        public String type
        {
#warning Snak type prefix better saved as an enum
            get;
            private set;
        }

        /// <summary>
        /// Gets the property id.
        /// </summary>
        /// <value>The property id.</value>
        public EntityId propertyId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public DataValue dataValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new <see cref="Snak"/> with the given values.
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="propertyId">The property id</param>
        /// <param name="dataValue">The data value</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyId"/> has a <see cref="EntityId.prefix"/> which is not the property prefix.</exception>
        public Snak(String type, EntityId propertyId, DataValue dataValue)
        {
            if ( propertyId == null )
                throw new ArgumentNullException("propertyId");

            if (propertyId.prefix != "p")
            {
                 throw new ArgumentException("propertyId must be a valid property id", "propertyId");
            }
            this.type = type;
            this.propertyId = propertyId;
            this.dataValue = dataValue;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected Snak()
        {
        }

        /// <summary>
        /// Fills the snak with data parsed from a JSon array.
        /// </summary>
        /// <param name="data">JSon array to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        protected virtual void fillFromArray(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            if (data.get("snaktype") == null || data.get("property") == null)
            {
                throw new ArgumentException("Invalid Snak serialization", "data");
            }
            this.type = data.get("snaktype").asString();
            this.propertyId = EntityId.newFromPrefixedId(data.get("property").asString());
            this.dataValue = DataValueFactory.newFromArray(data.get("datavalue").asObject());
        }

        internal static Snak newFromArray(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            var result = new Snak();
            result.fillFromArray(data);
            return result;
        }

        internal JsonObject toArray()
        {
            JsonObject data = new JsonObject()
                .add("snaktype", this.type)
                .add("property", this.propertyId.getPrefixedId());
            if (this.dataValue != null)
            {
                data.add("datavalue", this.dataValue.fullEncode());
            }
            return data;
        }
    }
}
