using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public struct AliasGroup
    {
        public readonly string Language;

        public readonly HashSet<string> Aliases;

        public AliasGroup(string language, HashSet<string> aliases)
        {
            if (language == null || aliases == null)
            {
                throw new ArgumentNullException("The language and the aliases must not be null");
            }

            Language = language;
            Aliases = new HashSet<string>(aliases);
        }
    }
}
