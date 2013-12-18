using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// A statement.
    /// </summary>
    public class Statement : Claim
    {
        /// <summary>
        /// The rank of the statement
        /// </summary>
        public string rank
        {
            get;
            private set;
        }

        /// <summary>
        /// Internal dictionary, storing the references by its <see cref="Reference.internalId"/>.
        /// </summary>
        private Dictionary<string, Reference> references = new Dictionary<string, Reference>();

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
        protected override void fillData(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            base.fillData(data);
            if (data.get("rank") != null)
            {
                this.rank = data.get("rank").asString();
            }
            if (data.get("references") != null)
            {
                foreach (JsonValue value in data.get("references").asArray())
                {
                    Reference reference = new Reference(this, value.asObject());
                    this.references.Add(reference.internalId, reference);
                }
            }
        }

        /// <summary>
        /// Gets all references.
        /// </summary>
        /// <returns>The references.</returns>
        [ObsoleteAttribute("Use References instead", false)]
        public Dictionary<string, Reference> getReferences()
        {
#warning Why create a new dictionary? The data values are not cloned. Besides, the key is inside the value already, so the References property below is the correct approach.
            return new Dictionary<string, Reference>(references);
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
        public void addReference(Reference reference)
        {
            if ( reference == null )
                throw new ArgumentNullException("reference");

            this.references[reference.internalId] = reference;
        }

        /// <summary>
        /// Remove a reference from the statement.
        /// </summary>
        /// <param name="reference">The reference to be removed.</param>
        /// <returns><c>true</c> if the reference was removed successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reference"/> is <c>null</c>.</exception>
        public bool removeReference(Reference reference)
        {
            if ( reference == null )
                throw new ArgumentNullException("reference");

            return this.references.Remove(reference.internalId);
        }

        /// <summary>
        /// Create a new reference in this statement with the provided snak.
        /// </summary>
        /// <param name="snak">The snak which makes up the reference.</param>
        /// <returns>The newly created reference.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="snak"/> is <c>null</c>.</exception>
        public Reference createReferenceForSnak(Snak snak)
        {
            if ( snak == null )
                throw new ArgumentNullException("snak");

            return Reference.newFromSnaks(this, new Snak[] { snak });
        }
    }
}
