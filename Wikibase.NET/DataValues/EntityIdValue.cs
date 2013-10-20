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
        /// <summary>
        /// The entity type
        /// </summary>
        public string entityType { get; set; }

        /// <summary>
        /// The numeric id
        /// </summary>
        public int numericId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <param name="numericId">The numeric id</param>
        public EntityIdValue(string entityType, int numericId)
        {
            this.entityType = entityType;
            this.numericId = numericId;
        }

        internal EntityIdValue(JsonValue value)
        {
            JsonObject obj = value.asObject();
            this.entityType = obj.get("entity-type").asString();
            this.numericId = obj.get("numeric-id").asInt();
        }

        public override string getType()
        {
            return "wikibase-entityid";
        }

        internal override JsonValue encode()
        {
            return new JsonObject()
                .add("entity-type", entityType)
                .add("numeric-id", numericId);
        }

        public override string ToString()
        {
            return entityType + " " + numericId;
        }
    }
}
