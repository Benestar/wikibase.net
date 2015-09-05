using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class CommonsMediaValue : DataValue
    {
        public const string URL_PREFIX = "https://commons.wikimedia.org/wiki/";

        public string Value { get; set; }

        public CommonsMediaValue(string value)
        {
            this.Value = value;
        }

        public string getUrl()
        {
            return URL_PREFIX + this.Value;
        }
    }
}
