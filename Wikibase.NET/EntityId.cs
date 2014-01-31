using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wikibase
{
    /// <summary>
    /// Supported entity types.
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// Properties, Wikidata URL is https://www.wikidata.org/wiki/Property:P###.
        /// </summary>
        Property,

        /// <summary>
        /// Items, Wikidata URL is https://www.wikidata.org/wiki/Q###.
        /// </summary>
        Item
    }

    /// <summary>
    /// Represents an ID of an Entity.
    /// </summary>
    public class EntityId
    {
        private Dictionary<EntityType, String> _entityTypePrefixes = new Dictionary<EntityType, String>
        {
            {EntityType.Item, "q"},
            {EntityType.Property, "p"},
        };

        /// <summary>
        /// The allowed prefixes for entity ids
        /// </summary>
        private static readonly String[] prefixes = new String[] { "q", "p" };

        /// <summary>
        /// Gets the entity type.
        /// </summary>
        /// <value>The entity type.</value>
        public EntityType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public String Prefix
        {
            get
            {
                return _entityTypePrefixes[Type];
            }
            private set
            {
                SetPrefix(value);
            }
        }

        /// <summary>
        /// Gets the numeric id.
        /// </summary>
        /// <value>The numeric id.</value>
        public Int32 NumericId
        {
            get;
            private set;
        }

        private static Regex prefixedIdRegex = new Regex(@"^(\w)(\d+)(#.*|)$");

        private void SetPrefix(String prefix)
        {
            var prefixToFind = prefix.ToLower(CultureInfo.InvariantCulture);
            if ( !_entityTypePrefixes.Values.Contains(prefixToFind) )
            {
                throw new ArgumentException(String.Format("\"{0}\" is no recognized prefix", prefix));
            }
            this.Type = _entityTypePrefixes.First(x => x.Value == prefixToFind).Key;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="numericId">The numeric id</param>
        public EntityId(String prefix, Int32 numericId)
        {
            this.Prefix = prefix;
            this.NumericId = numericId;
        }

        /// <summary>
        /// Constructs an entity id from a prefixed id.
        /// </summary>
        /// <param name="prefixedId">The prefixed id.</param>
        public EntityId(String prefixedId)
        {
            Boolean success = false;
            if ( !String.IsNullOrWhiteSpace(prefixedId) )
            {
                Match match = prefixedIdRegex.Match(prefixedId.ToLower(CultureInfo.InvariantCulture));
                if ( match.Success )
                {
                    if ( Array.Exists(prefixes, delegate(String s)
                {
                    return s == match.Groups[1].Value;
                }) )
                    {
                        this.NumericId = Int32.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                        this.Prefix = match.Groups[1].Value;
                        success = true;
                    }
                }
            }
            if ( !success )
            {
                throw new ArgumentException(String.Format("\"{0}\" is not a parseable prefixed id", prefixedId));
            }
        }

        /// <summary>
        /// Gets the prefixed id of the entity id.
        /// </summary>
        /// <value>The prefixed id.</value>
        public String PrefixedId
        {
            get
            {
                return Prefix + NumericId;
            }
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
            return Type == otherId.Type && NumericId == otherId.NumericId;
        }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return (Type.GetHashCode() * 3) ^ (NumericId.GetHashCode() * 7);
        }

        /// <summary>
        /// Converts a entity Id to a string.
        /// </summary>
        /// <returns>Entity Id as a string.</returns>
        public override String ToString()
        {
            return PrefixedId;
        }
    }
}