using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// A data value
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

        private string md5(string text)
        {
            if ((text == null) || (text.Length == 0))
            {
                return string.Empty;
            }
            byte[] result = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(text));
            return System.BitConverter.ToString(result);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            DataValue other = (DataValue)obj;
            return this.encode() == other.encode();
        }

        public override int GetHashCode()
        {
            return this.encode().GetHashCode();
        }

        public override string ToString()
        {
            return this.encode().ToString();
        }

        /// <summary>
        /// Get the type of the data value.
        /// </summary>
        /// <returns></returns>
        public abstract string getType();

        /// <summary>
        /// Encode the data value to json.
        /// </summary>
        /// <returns>The json value</returns>
        internal abstract JsonValue encode();
    }
}
