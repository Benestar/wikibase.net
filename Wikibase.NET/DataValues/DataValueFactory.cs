using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    static class DataValueFactory
    {
        internal static DataValue newFromArray(JsonObject data)
        {
            return newDataValue(data.get("type").asString(), data.get("value"));
        }

        internal static DataValue newDataValue(string type, JsonValue value)
        {
            switch (type)
            {
                case "wikibase-entityid":
                    return new EntityIdValue(value);
                case "string":
                    return new StringValue(value);
                case "time":
                    return new TimeValue(value);
                case "globecoordinate":
                    return new GlobeCoordinateValue(value);
                default:
                    throw new NotSupportedException("Unsupported type " + type);
            }
        }
    }
}
