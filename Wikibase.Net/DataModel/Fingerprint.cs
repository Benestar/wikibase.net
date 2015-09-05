using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class Fingerprint
    {
        public IndexedList<string, Term> Labels { get; private set; }

        public IndexedList<string, Term> Descriptions { get; private set; }

        public IndexedList<string, AliasGroup> Aliases { get; private set; }

        public bool Empty
        {
            get
            {
                return Labels.Count == 0 && Descriptions.Count == 0 && Aliases.Count == 0;
            }
        }

        public Fingerprint(List<Term> labels = null, List<Term> descriptions = null, List<AliasGroup> aliases = null)
        {
            Labels = new IndexedList<string, Term>(term => term.Language, labels);
            Descriptions = new IndexedList<string, Term>(term => term.Language, descriptions);
            Aliases = new IndexedList<string, AliasGroup>(aliasGroup => aliasGroup.Language, aliases);
        }
    }
}
