using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Abstract base class for a data value.
    /// </summary>
    public abstract class DataValue
    {
        #region Json names

        /// <summary>
        /// Json name for the datavalue type.
        /// </summary>
        public const String ValueTypeJsonName = "type";

        /// <summary>
        /// Json name for the datavalue content.
        /// </summary>
        public const String ValueJsonName = "value";

        #endregion Json names

        /// <summary>
        /// Get the hash.
        /// </summary>
        /// <returns>The hash.</returns>
        public String getHash()
        {
            return md5(this.Encode().ToString());
        }

        private static String md5(String text)
        {
            if ( (text == null) || (text.Length == 0) )
            {
                return String.Empty;
            }
            Byte[] result;
            MD5CryptoServiceProvider md5provider = null;
            try
            {
                md5provider = new MD5CryptoServiceProvider();
                result = md5provider.ComputeHash(Encoding.Default.GetBytes(text));
            }
            finally
            {
                if ( md5provider != null )
                    md5provider.Dispose();
            }
            return System.BitConverter.ToString(result);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same
        /// type.
        ///</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object other)
        {
            if ( this == other )
            {
                return true;
            }
            if ( other == null )
            {
                return false;
            }
            if ( this.GetType() != other.GetType() )
            {
                return false;
            }
            DataValue otherDataValue = (DataValue)other;
            return this.Encode() == otherDataValue.Encode();
        }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return this.Encode().GetHashCode();
        }

        /// <summary>
        /// Converts the instance to a string.
        /// </summary>
        /// <returns>String representation of the instance.</returns>
        public override String ToString()
        {
            return this.Encode().ToString();
        }

        /// <summary>
        /// Get the type of the data value.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        protected abstract String JsonName();

        /// <summary>
        /// Encode the value part of the data value to json.
        /// </summary>
        /// <returns>The json value.</returns>
        internal abstract JsonValue Encode();

        /// <summary>
        /// Encode the data value to json.
        /// </summary>
        /// <returns>The json value.</returns>
        internal JsonValue fullEncode()
        {
            JsonObject data = new JsonObject()
                .add(ValueTypeJsonName, this.JsonName())
                .add(ValueJsonName, this.Encode());
            return data;
        }
    }
}