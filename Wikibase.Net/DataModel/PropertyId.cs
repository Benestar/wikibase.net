using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class PropertyId : EntityId
    {
        protected override string regex { get { return "^P[1-9][0-9]*$"; } }

        public PropertyId(string serialization) : base(serialization) {}
    }
}
