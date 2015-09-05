
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class PropertySomeValueSnak : Snak
    {
        public PropertyId PropertyId { get; private set; }

        public SnakType Type { get { return SnakType.SomeValueSnak; } }

        public PropertySomeValueSnak(PropertyId propertyId)
        {
            if (propertyId == null)
            {
                throw new ArgumentNullException("The property id must not be null");
            }

            PropertyId = propertyId;
        }

        public PropertySomeValueSnak(string idSerialization)
            : this(new PropertyId(idSerialization)) { }
    }
}
