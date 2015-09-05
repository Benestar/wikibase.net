using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase.DataModel.DataValues;

namespace Wikibase.DataModel
{
    public class PropertyValueSnak : Snak
    {
        public PropertyId PropertyId { get; private set; }

        public SnakType Type { get { return SnakType.ValueSnak; } }

        public DataValue Value { get; private set; }

        public PropertyValueSnak(PropertyId propertyId, DataValue value)
        {
            if (propertyId == null || value == null)
            {
                throw new ArgumentNullException("The property id and value must not be null");
            }

            PropertyId = propertyId;
            Value = value;
        }

        public PropertyValueSnak(string idSerialization, DataValue value)
            : this(new PropertyId(idSerialization), value) { }
    }
}
