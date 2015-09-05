using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class QuantityValue : DataValue
    {
        public double Value { get; set; }

        public QuantityValue(double value)
        {
            this.Value = value;
        }
    }
}
