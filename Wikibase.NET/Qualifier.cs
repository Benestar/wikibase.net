using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinimalJson;
using Wikibase.DataValues;

namespace Wikibase
{
    /// <summary>
    /// Qualifier, almost identical to a <see cref="Snak"/>.
    /// </summary>
    public class Qualifier : Snak
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="propertyId">The property id</param>
        /// <param name="dataValue">The data value</param>
        public Qualifier(string type, EntityId propertyId, DataValue dataValue)
            : base(type, propertyId, dataValue)
        {
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected Qualifier()
            : base()
        {
        }

        /// <summary>
        /// Creates a <see cref="Qualifier"/> from a <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="data">JSonObject to be parsed.</param>
        /// <returns>Newly created qualifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        new internal static Qualifier newFromArray(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            var result = new Qualifier();
            result.fillFromArray(data);
            return result;
        }
    }
}
