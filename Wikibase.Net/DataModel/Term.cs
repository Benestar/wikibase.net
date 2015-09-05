using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public struct Term
    {
        public readonly string Language;

        public readonly string Text;

        public Term(string language, string text)
        {
            if (language == null || text == null)
            {
                throw new ArgumentNullException("The language and the text must not be null");
            }

            Language = language;
            Text = text;
        }
    }
}
