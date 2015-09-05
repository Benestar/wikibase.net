using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class ItemId : EntityId
    {
        protected override string regex { get { return "^Q[1-9][0-9]*$"; } }

        public ItemId(string serialization) : base(serialization) { }
    }
}
