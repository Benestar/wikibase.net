using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase.DataModel.DataValues;

namespace Wikibase.DataModel
{
    public enum SnakType
    {
        ValueSnak, NoValueSnak, SomeValueSnak
    }

    public interface Snak
    {
        PropertyId PropertyId { get; }

        SnakType Type { get; }
    }
}
