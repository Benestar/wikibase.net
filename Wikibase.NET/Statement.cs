using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// Ranks of statements.
    /// </summary>
    public enum Rank
    {
        /// <summary>
        /// Rank not defined.
        /// </summary>
        Unknown,

        /// <summary>
        /// Preferred statement.
        /// </summary>
        Preferred,

        /// <summary>
        /// Normal statement.
        /// </summary>
        Normal,

        /// <summary>
        /// Deprecated statement.
        /// </summary>
        Deprecated,
    }

    /// <summary>
    /// A statement.
    /// </summary>
    public class Statement : Claim
    {
        #region Jscon names

        /// <summary>
        /// The name of the <see cref="References"/> property in the serialized json object.
        /// </summary>
        private const String ReferencesJsonName = "references";

        /// <summary>
        /// The name of the <see cref="Rank"/> property in the serialized json object.
        /// </summary>
        private const String RankJsonName = "rank";

        private static Dictionary<Rank, String> _rankJsonNames = new Dictionary<Rank, String>()
        {
             {Rank.Preferred, "preferred" },
             {Rank.Normal, "normal" },
             {Rank.Deprecated, "deprecated" }
        };

        #endregion Jscon names

        /// <summary>
        /// Gets the rank of the statement.
        /// </summary>
        /// <value>The rank of the statement.</value>
        public Rank Rank
        {
            get;
            private set;
        }

        /// <summary>
        /// Internal dictionary, storing the references by its <see cref="Reference.InternalId"/>.
        /// </summary>
        private Dictionary<String, Reference> references = new Dictionary<String, Reference>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="entity">Entity to which the statement belongs.</param>
        /// <param name="data">JSon data to be parsed.</param>
        internal Statement(Entity entity, JsonObject data)
            : base(entity, data)
        {
        }

        /// <summary>
        /// Parses the <paramref name="data"/> and adds the results to this instance.
        /// </summary>
        /// <param name="data"><see cref="JsonObject"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        protected override void FillData(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            base.FillData(data);
            if ( data.get(RankJsonName) != null )
            {
                var rank = data.get(RankJsonName).asString();
                if ( _rankJsonNames.Any(x => x.Value == rank) )
                {
                    this.Rank = _rankJsonNames.First(x => x.Value == rank).Key;
                }
                else
                {
                    this.Rank = Rank.Unknown;
                }
            }
            if ( data.get(ReferencesJsonName) != null )
            {
                foreach ( JsonValue value in data.get(ReferencesJsonName).asArray() )
                {
                    Reference reference = new Reference(this, value.asObject());
                    this.references[reference.InternalId]= reference;
                }
            }
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>The references.</value>
        public IEnumerable<Reference> References
        {
            get
            {
                return references.Values;
            }
        }

        /// <summary>
        /// Add the reference.
        /// </summary>
        /// <param name="reference">The reference to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reference"/> is <c>null</c>.</exception>
        public void AddReference(Reference reference)
        {
            if ( reference == null )
                throw new ArgumentNullException("reference");

            this.references[reference.InternalId] = reference;
        }

        /// <summary>
        /// Remove a reference from the statement.
        /// </summary>
        /// <param name="reference">The reference to be removed.</param>
        /// <returns><c>true</c> if the reference was removed successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reference"/> is <c>null</c>.</exception>
        public Boolean RemoveReference(Reference reference)
        {
            if ( reference == null )
                throw new ArgumentNullException("reference");

            return this.references.Remove(reference.InternalId);
        }

        /// <summary>
        /// Create a new reference in this statement with the provided snak.
        /// </summary>
        /// <param name="snak">The snak which makes up the reference.</param>
        /// <returns>The newly created reference.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="snak"/> is <c>null</c>.</exception>
        public Reference CreateReferenceForSnak(Snak snak)
        {
            if ( snak == null )
                throw new ArgumentNullException("snak");

            return new Reference(this, new Snak[] { snak });
        }
    }
}