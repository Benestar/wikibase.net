using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Data value for entity ids
    /// </summary>
    public class EntityIdValue : DataValue
    {
#warning Make properties read-only - changing them will not save to the changes of the claim
#warning Shouldn't this class simply contain a EntityId, which already encapsulates entityType and numericId

        /// <summary>
        /// Gets or sets the entity type
        /// </summary>
        /// <value>The entity type.</value>
        /// <remarks>Should be "item" in most cases.</remarks>
        public String entityType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the numeric id.
        /// </summary>
        /// <value>The numeric id.</value>
        public Int32 numericId
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entityType">The entity type ("item").</param>
        /// <param name="numericId">The numeric id.</param>
        public EntityIdValue(String entityType, Int32 numericId)
        {
            this.entityType = entityType;
            this.numericId = numericId;
        }

        /// <summary>
        /// Creates a new instance by parsing a JsonValue.
        /// </summary>
        /// <param name="value">JSonValue to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        internal EntityIdValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            JsonObject obj = value.asObject();
            this.entityType = obj.get("entity-type").asString();
            this.numericId = obj.get("numeric-id").asInt();
        }

        /// <summary>
        /// Get the type of the data value.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        public override string getType()
        {
            return "wikibase-entityid";
        }

        /// <summary>
        /// Encode the value part of the data value to json.
        /// </summary>
        /// <returns>The json value</returns>
        internal override JsonValue encode()
        {
            return new JsonObject()
                .add("entity-type", entityType)
                .add("numeric-id", numericId);
        }

        /// <summary>
        /// Converts the instance to a string.
        /// </summary>
        /// <returns>String representation of the instance.</returns>
        public override String ToString()
        {
            return entityType + " " + numericId;
        }
    }
}