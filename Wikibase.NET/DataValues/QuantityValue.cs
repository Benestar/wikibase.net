using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Possible unit values for the <see cref="QuantityValue.Unit"/>.
    /// </summary>
    public enum QuantityUnit
    {
        /// <summary>
        /// Undefined value.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Number without a dimension.
        /// </summary>
        DimensionLess = 1,
    }

    /// <summary>
    /// Encapsulates the quantity value type.
    /// </summary>
    public class QuantityValue : DataValue
    {
        #region Jscon names

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

        #endregion Jscon names

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
        /// Gets or sets the unit of measurement.
        /// </summary>
        /// <value>The unit of measurement.</value>
        public QuantityUnit Unit
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
        /// Creates a new quantity value for a exact integer value.
        /// </summary>
        /// <param name="value">Integer value.</param>
        public QuantityValue(Int64 value)
        {
            Amount = value.ToString(CultureInfo.InvariantCulture);
            if ( value > 0 )
            {
                Amount = "+" + Amount;
            }
            UpperBound = Amount;
            LowerBound = Amount;
            Unit = QuantityUnit.DimensionLess;
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
            this.Unit = (QuantityUnit)Convert.ToInt32(obj.get(UnitJsonName).asString());
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
                .add(UnitJsonName, Convert.ToInt32(Unit).ToString(CultureInfo.InvariantCulture))
                .add(UpperBoundJsonName, UpperBound)
                .add(LowerBoundJsonName, LowerBound);
        }
    }
}