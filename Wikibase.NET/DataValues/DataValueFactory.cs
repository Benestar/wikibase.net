using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Factory to create the correct <see cref="DataValue"/> from a <see cref="JsonObject"/>.
    /// </summary>
    internal static class DataValueFactory
    {
        internal static DataValue CreateFromJsonObject(JsonObject data)
        {
            return CreateFromJsonValue(data.get(DataValue.ValueTypeJsonName).asString(), data.get(DataValue.ValueJsonName));
        }

        internal static DataValue CreateFromJsonValue(String type, JsonValue value)
        {
            switch ( type )
            {
                case EntityIdValue.TypeJsonName:
                    return new EntityIdValue(value);
                case StringValue.TypeJsonName:
                    return new StringValue(value);
                case TimeValue.TypeJsonName:
                    return new TimeValue(value);
                case GlobeCoordinateValue.TypeJsonName:
                    return new GlobeCoordinateValue(value);
                case QuantityValue.TypeJsonName:
                    return new QuantityValue(value);
                case MonolingualTextValue.TypeJsonName:
                    return new MonolingualTextValue(value);
                default:
                    throw new NotSupportedException("Unsupported type " + type);
            }
        }
    }
}