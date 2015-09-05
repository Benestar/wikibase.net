using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class DateTimeValue : DataValue
    {
        public DateTime Value { get; set; }

        public DateTimeValue(DateTime value)
        {
            this.Value = value;
        }
    }
}
