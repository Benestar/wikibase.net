using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class StringValue : DataValue
    {
        public string Value { get; set; }

        public StringValue(string value)
        {
            this.Value = value;
        }
    }
}
