using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Encapsulates the string value type.
    /// </summary>
    public class StringValue : DataValue
    {
        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public String str
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of <see cref="StringValue"/> with the given value.
        /// </summary>
        /// <param name="value">Value to be added.</param>
        public StringValue(String value)
        {
            this.str = value;
        }

        /// <summary>
        /// Parses a <see cref="JsonValue"/> to a <see cref="StringValue"/>
        /// </summary>
        /// <param name="value"><see cref="JsonValue"/> to be parsed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        internal StringValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            this.str = value.asString();
        }

        /// <summary>
        /// Gets the data type identifier.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        public override String getType()
        {
            return "string";
        }

        /// <summary>
        /// Encodes the instance in a <see cref="JsonValue"/>.
        /// </summary>
        /// <returns>Encoded instance.</returns>
        internal override JsonValue encode()
        {
            return JsonValue.valueOf(str);
        }
    }
}
