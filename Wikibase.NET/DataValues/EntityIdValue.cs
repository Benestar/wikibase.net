using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Data value for entity ids
    /// </summary>
    public class EntityIdValue : DataValue
    {
        #region Json names

        /// <summary>
        /// The identifier of this data type in the serialized json object.
        /// </summary>
        public const String TypeJsonName = "wikibase-entityid";

        /// <summary>
        /// The name of the <see cref="NumericId"/> property in the serialized json object.
        /// </summary>
        private const String NumericIdJsonName = "numeric-id";

        /// <summary>
        /// The name of the <see cref="EntityType"/> property in the serialized json object.
        /// </summary>
        private const String EntityTypeJsonName = "entity-type";

        #endregion Json names

        private Dictionary<EntityType, String> _entityTypeJsonNames = new Dictionary<EntityType, String>()
        {
             {EntityType.Property, "property" },
             {EntityType.Item, "item"}
        };

#warning Shouldn't this class simply contain a EntityId, which already encapsulates entityType and numericId

        /// <summary>
        /// Gets or sets the entity type
        /// </summary>
        /// <value>The entity type.</value>
        /// <remarks>Should be "item" in most cases.</remarks>
        public EntityType EntityType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the numeric id.
        /// </summary>
        /// <value>The numeric id.</value>
        public Int32 NumericId
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entityType">The entity type ("item").</param>
        /// <param name="numericId">The numeric id.</param>
        public EntityIdValue(EntityType entityType, Int32 numericId)
        {
            this.EntityType = entityType;
            this.NumericId = numericId;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public EntityIdValue(EntityId entityId)
        {
            this.EntityType = entityId.Type;
            this.NumericId = entityId.NumericId;
        }

        /// <summary>
        /// Creates a new instance by parsing a JsonValue.
        /// </summary>
        /// <param name="value">JSonValue to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> contains data which cannot be parsed.</exception>
        internal EntityIdValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            JsonObject obj = value.asObject();
            var entityTypeJson = obj.get(EntityTypeJsonName).asString();
            if ( !_entityTypeJsonNames.Any(x => x.Value == entityTypeJson) )
            {
                throw new ArgumentException(String.Format("Json contained unknown entity type {0}", entityTypeJson));
            }
            this.EntityType = _entityTypeJsonNames.First(x => x.Value == entityTypeJson).Key;
            this.NumericId = obj.get(NumericIdJsonName).asInt();
        }

        /// <summary>
        /// Get the type of the data value.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        protected override String JsonName()
        {
            return TypeJsonName;
        }

        /// <summary>
        /// Encode the value part of the data value to json.
        /// </summary>
        /// <returns>The json value</returns>
        internal override JsonValue Encode()
        {
            return new JsonObject()
                .add(EntityTypeJsonName, _entityTypeJsonNames[EntityType])
                .add(NumericIdJsonName, NumericId);
        }

        /// <summary>
        /// Converts the instance to a string.
        /// </summary>
        /// <returns>String representation of the instance.</returns>
        public override String ToString()
        {
            return EntityType + " " + NumericId;
        }
    }
}