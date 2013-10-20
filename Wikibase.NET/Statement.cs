using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// A statement
    /// </summary>
    public class Statement : Claim
    {
        /// <summary>
        /// The rank of the statement
        /// </summary>
        public string rank { get; private set; }

        private Dictionary<string, Reference> references = new Dictionary<string, Reference>();

        internal Statement(Entity entity, JsonObject data) : base(entity, data) { }

        protected override void fillData(JsonObject data)
        {
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
        /// Get all references.
        /// </summary>
        /// <returns>The references</returns>
        public Dictionary<string, Reference> getReferences()
        {
            return new Dictionary<string, Reference>(references);
        }

        /// <summary>
        /// Add the reference.
        /// </summary>
        /// <param name="reference">The reference</param>
        public void addReference(Reference reference)
        {
            this.references[reference.internalId] = reference;
        }

        /// <summary>
        /// Remove the reference.
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <returns>If the reference was removed successfully</returns>
        public bool removeReference(Reference reference)
        {
            return this.references.Remove(reference.internalId);
        }

        /// <summary>
        /// Create a new reference in this statement for the provided snak.
        /// </summary>
        /// <param name="snak">The snak</param>
        /// <returns>The reference</returns>
        public Reference createReferenceForSnak(Snak snak)
        {
            return Reference.newFromSnaks(this, new Snak[] { snak });
        }
    }
}
