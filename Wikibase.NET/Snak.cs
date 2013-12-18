using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;
using Wikibase.DataValues;

namespace Wikibase
{
    /// <summary>
    /// A snak
    /// </summary>
    public class Snak
    {
        /// <summary>
        /// The type
        /// </summary>
        public string type { get; private set; }

        /// <summary>
        /// The property id
        /// </summary>
        public EntityId propertyId { get; private set; }

        /// <summary>
        /// The data value
        /// </summary>
        public DataValue dataValue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="propertyId">The property id</param>
        /// <param name="dataValue">The data value</param>
        public Snak(string type, EntityId propertyId, DataValue dataValue)
        {
            if (propertyId.prefix != "p")
            {
                 throw new ArgumentException("propertyId must be a valid property id", "propertyId");
            }
            this.type = type;
            this.propertyId = propertyId;
            this.dataValue = dataValue;
        }

        internal static Snak newFromArray(JsonObject data)
        {
            if (data.get("snaktype") == null || data.get("property") == null)
            {
                throw new ArgumentException("Invalid Snak serialization", "data");
            }
            return new Snak(
                data.get("snaktype").asString(),
                EntityId.newFromPrefixedId(data.get("property").asString()),
                DataValueFactory.newFromArray(data.get("datavalue").asObject())
            );
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
