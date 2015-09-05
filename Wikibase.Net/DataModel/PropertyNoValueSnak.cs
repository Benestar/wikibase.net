using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class PropertyNoValueSnak : Snak
    {
        public PropertyId PropertyId { get; private set; }

        public SnakType Type { get { return SnakType.NoValueSnak; } }

        public PropertyNoValueSnak(PropertyId propertyId)
        {
            if (propertyId == null)
            {
                throw new ArgumentNullException("The property id must not be null");
            }

            PropertyId = propertyId;
        }

        public PropertyNoValueSnak(string idSerialization)
            : this(new PropertyId(idSerialization)) { }
    }
}
