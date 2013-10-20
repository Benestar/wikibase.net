using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    public class StringValue : DataValue
    {
        public string str { get; set; }

        public StringValue(string str)
        {
            this.str = str;
        }

        internal StringValue(JsonValue value)
        {
            this.str = value.asString();
        }

        public override string getType()
        {
            return "string";
        }

        internal override JsonValue encode()
        {
            return JsonValue.valueOf(str);
        }
    }
}
