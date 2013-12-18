using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Wikibase
{
    /// <summary>
    /// Represents an ID of an Entity.
    /// </summary>
    public class EntityId
    {
        /// <summary>
        /// The allowed prefixes for entity ids
        /// </summary>
        public static readonly string[] prefixes = new string[] { "q", "p" };

        /// <summary>
        /// The prefix
        /// </summary>
        public string prefix
        {
            get;
            private set;
        }

        /// <summary>
        /// The numeric
        /// </summary>
        public int numericId
        {
            get;
            private set;
        }

        private static Regex prefixedIdRegex = new Regex(@"^(\w)(\d+)(#.*|)$");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="numericId">The numeric id</param>
        public EntityId(string prefix, int numericId)
        {
            this.prefix = prefix;
            this.numericId = numericId;
        }

        /// <summary>
        /// Constructs an entity id from a prefixed id.
        /// </summary>
        /// <param name="prefixedId">The prefixed id</param>
        /// <returns>The entity id</returns>
        public static EntityId newFromPrefixedId(String prefixedId)
        {
            if ( !String.IsNullOrWhiteSpace(prefixedId) )
            {
                Match match = prefixedIdRegex.Match(prefixedId.ToLower(CultureInfo.InvariantCulture));
                if ( match.Success )
                {
                    if ( Array.Exists(prefixes, delegate(string s)
                {
                    return s == match.Groups[1].Value;
                }) )
                    {
                        return new EntityId(match.Groups[1].Value, int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the prefixed id of the entity id.
        /// </summary>
        /// <returns>The prefixed id</returns>
        public string getPrefixedId()
        {
            return prefix + numericId;
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
            EntityId otherId = (EntityId)other;
            return prefix == otherId.prefix && numericId == otherId.numericId;
        }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return (prefix.GetHashCode() * 3) ^ (numericId.GetHashCode() * 7);
        }

        /// <summary>
        /// Converts a entity Id to a string.
        /// </summary>
        /// <returns>Entity Id as a string.</returns>
        public override String ToString()
        {
            return prefix + numericId;
        }
    }
}