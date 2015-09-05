using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel.DataValues
{
    public class ItemIdValue : DataValue
    {
        public ItemId Value { get; set; }

        public ItemIdValue(ItemId value)
        {
            this.Value = value;
        }
    }
}
