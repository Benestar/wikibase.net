using System;
using System.Collections.Generic;
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
        public string prefix { get; private set; }

        /// <summary>
        /// The numeric
        /// </summary>
        public int numericId { get; private set; }

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
        public static EntityId newFromPrefixedId(string prefixedId)
        {
            Match match = prefixedIdRegex.Match(prefixedId.ToLower());
            if (match.Success)
            {
                if (Array.Exists(prefixes, delegate(string s) { return s == match.Groups[1].Value; }))
                {
                    return new EntityId(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
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
            EntityId other = (EntityId) obj;
            return prefix == other.prefix && numericId == other.numericId;
        }

        public override int GetHashCode()
        {
            return (prefix.GetHashCode() * 3) ^ (numericId.GetHashCode() * 7);
        }

        public override string ToString()
        {
            return prefix + numericId;
        }
    }
}
