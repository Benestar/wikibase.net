using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public enum StatementRank
    {
        Preferred, Normal, Deprecated
    }

    public class Statement
    {
        public string Guid { get; private set; }

        public Snak Snak { get; private set; }

        public List<Snak> Qualifiers { get; private set; }

        public List<Snak> References { get; private set; }

        public PropertyId PropertyId { get { return Snak.PropertyId; } }

        public StatementRank Rank { get; set; }

        public Statement(string guid, Snak snak, List<Snak> qualifiers = null, List<Snak> references = null, StatementRank rank = StatementRank.Normal)
        {
            if (snak == null)
            {
                throw new ArgumentNullException("The snak must not be null");
            }

            Guid = guid;
            Snak = snak;
            Qualifiers = qualifiers == null ? new List<Snak>() : qualifiers;
            References = references == null ? new List<Snak>() : references;
            Rank = rank;
        }

        public Statement(Snak snak, List<Snak> qualifiers = null, List<Snak> references = null)
            : this(null, snak, qualifiers, references) { }
    }
}
