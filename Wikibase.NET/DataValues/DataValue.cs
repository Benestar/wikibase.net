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
        /// <summary>
        /// Get the hash.
        /// </summary>
        /// <returns>The hash</returns>
        public string getHash()
        {
            return md5(this.encode().ToString());
        }

        private static string md5(string text)
        {
            if ( (text == null) || (text.Length == 0) )
            {
                return string.Empty;
            }
            byte[] result;
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
        public override Boolean Equals(object other)
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
            return this.encode() == otherDataValue.encode();
        }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return this.encode().GetHashCode();
        }

        /// <summary>
        /// Converts the instance to a string.
        /// </summary>
        /// <returns>String representation of the instance.</returns>
        public override String ToString()
        {
            return this.encode().ToString();
        }

        /// <summary>
        /// Get the type of the data value.
        /// </summary>
        /// <returns>Data type identifier.</returns>
        public abstract string getType();

        /// <summary>
        /// Encode the value part of the data value to json.
        /// </summary>
        /// <returns>The json value</returns>
        internal abstract JsonValue encode();

        /// <summary>
        /// Encode the data value to json.
        /// </summary>
        /// <returns>The json value</returns>
        internal JsonValue fullEncode()
        {
            JsonObject data = new JsonObject()
                .add("type", this.getType())
                .add("value", this.encode());
            return data;
        }
    }
}