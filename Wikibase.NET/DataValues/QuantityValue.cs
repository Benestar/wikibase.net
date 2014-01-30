using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Encapsulates the quantity value type.
    /// </summary>
    public class QuantityValue : DataValue
    {
        /// <summary>
        /// The identifier of this data type in the serialized json object.
        /// </summary>
        public const String TypeJsonName = "quantity";

        /// <summary>
        /// The name of the <see cref="UpperBound"/> property in the serialized json object.
        /// </summary>
        private const String UpperBoundJsonName = "upperBound";

        /// <summary>
        /// The name of the <see cref="LowerBound"/> property in the serialized json object.
        /// </summary>
        private const String LowerBoundJsonName = "lowerBound";

        /// <summary>
        /// The name of the <see cref="Amount"/> property in the serialized json object.
        /// </summary>
        private const String AmountJsonName = "amount";

        /// <summary>
        /// The name of the <see cref="Unit"/> property in the serialized json object.
        /// </summary>
        private const String UnitJsonName = "unit";

        // TODO: Better data structures, string is too general

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public String Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public String Unit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public String UpperBound
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public String LowerBound
        {
            get;
            set;
        }

        /// <summary>
        /// Parses a <see cref="JsonValue"/> to a <see cref="QuantityValue"/>
        /// </summary>
        /// <param name="value"><see cref="JsonValue"/> to be parsed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        internal QuantityValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            JsonObject obj = value.asObject();
            this.Amount = obj.get(AmountJsonName).asString();
            this.Unit = obj.get(UnitJsonName).asString();
            this.UpperBound = obj.get(UpperBoundJsonName).asString();
            this.LowerBound = obj.get(LowerBoundJsonName).asString();
        }

        /// <summary>
        /// Gets the data type identifier.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        protected override String JsonName()
        {
            return TypeJsonName;
        }

        /// <summary>
        /// Encodes the instance in a <see cref="JsonValue"/>.
        /// </summary>
        /// <returns>Encoded instance.</returns>
        internal override JsonValue Encode()
        {
            return new JsonObject()
                .add(AmountJsonName, Amount)
                .add(UnitJsonName, Unit)
                .add(UpperBoundJsonName, UpperBound)
                .add(LowerBoundJsonName, LowerBound);
        }
    }
}
