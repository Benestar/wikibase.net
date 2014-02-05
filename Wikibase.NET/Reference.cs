using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;
using System.Linq;

namespace Wikibase
{
    /// <summary>
    /// A reference.
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// Gets the statement this reference belongs to.
        /// </summary>
        /// <value>The statment this reference belongs to.</value>
        public Statement statement { get; private set; }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public String hash { get; private set; }

        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <value>The internal id.</value>
        public String internalId { get; private set; }

        private Dictionary<String, Dictionary<String, Snak>> snaks = new Dictionary<String, Dictionary<String, Snak>>();

        /// <summary>
        /// Creates a new reference by parsing the JSon result.
        /// </summary>
        /// <param name="statement">Statement to which the new reference belongs.</param>
        /// <param name="data">JsonObject to parse.</param>
        internal Reference(Statement statement, JsonObject data)
        {
            this.statement = statement;
            this.fillData(data);
        }

        /// <summary>
        /// Parses the <paramref name="data"/> and adds the results to this instance.
        /// </summary>
        /// <param name="data"><see cref="JsonObject"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        protected void fillData(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            if (data.get("snaks") != null)
            {
                foreach (JsonObject.Member member in data.get("snaks").asObject())
                {
                    Dictionary<string, Snak> list = new Dictionary<string, Snak>();
                    foreach (JsonValue value in member.value.asArray())
                    {
                        Snak snak = new Snak(value.asObject());
                        list.Add(snak.DataValue.getHash(), snak);
                    }
                    this.snaks.Add(member.name, list);
                }
            }
            if (data.get("hash") != null)
            {
                this.hash = data.get("hash").asString();
            }
            if (this.internalId == null)
            {
                if (this.hash != null)
                {
                    this.internalId = this.hash;
                }
                else
                {
                    this.internalId = "" + Environment.TickCount + this.statement.internalId;
                }
            }
        }

        /// <summary>
        /// Create a new references with the given snaks.
        /// </summary>
        /// <param name="statement">Statement to which the reference should be added.</param>
        /// <param name="snaks">Snaks to be part of the reference.</param>
        /// <returns>New reference instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="statement"/> or <paramref name="snaks"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="snaks"/> is empty.</exception>
        public static Reference newFromSnaks(Statement statement, Snak[] snaks)
        {
            if ( snaks == null )
                throw new ArgumentNullException("snaks");
            if ( statement  == null )
                throw new ArgumentNullException("statement");
            if ( !snaks.Any())
                throw new ArgumentException("snaks");

            Reference reference = new Reference(statement, new JsonObject());
            foreach (Snak snak in snaks)
            {
                reference.addSnak(snak);
            }
            statement.addReference(reference);
            return reference;
        }

        /// <summary>
        /// Get all snaks.
        /// </summary>
        /// <returns>The snaks</returns>
        /// <remarks>Key is the prefixed property Id, value is a dictionary with <see cref="Wikibase.DataValues.DataValue.getHash"/> as key and the actual snak as value.</remarks>
        public Dictionary<String, Dictionary<String, Snak>> getSnaks()
        {
            Dictionary<String, Dictionary<String, Snak>> copy = new Dictionary<String, Dictionary<String, Snak>>(snaks);
            foreach (KeyValuePair<String, Dictionary<String, Snak>> pair in snaks)
            {
                copy[pair.Key] = new Dictionary<String, Snak>(pair.Value);
            }
            return copy;
        }

        /// <summary>
        /// Add a snak.
        /// </summary>
        /// <param name="snak">The snak.</param>
        /// <exception cref="ArgumentNullException"><paramref name="snak"/> is <c>null</c>.</exception>
        public void addSnak(Snak snak)
        {
            if ( snak == null )
                throw new ArgumentNullException("snak");

            String property = snak.PropertyId.PrefixedId;
            if (!this.snaks.ContainsKey(property))
            {
                this.snaks[property] = new Dictionary<String, Snak>();
            }
            this.snaks[property][snak.DataValue.getHash()] = snak;
        }

        /// <summary>
        /// Remove the snak.
        /// </summary>
        /// <param name="snak">The snak.</param>
        /// <returns><c>true</c> if the snak was removed successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="snak"/> is <c>null</c>.</exception>
        public Boolean removeSnak(Snak snak)
        {
            if ( snak == null )
                throw new ArgumentNullException("snak");

            String property = snak.PropertyId.PrefixedId;
            if (!this.snaks.ContainsKey(property))
            {
                return false;
            }
            if (this.snaks[property].Remove(snak.DataValue.getHash()))
            {
                if (this.snaks[property].Count == 0)
                {
                    this.snaks.Remove(property);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save the reference.
        /// </summary>
        /// <param name="summary">The summary</param>
        /// <exception cref="InvalidOperationException">Statement has no id because not saved yet.</exception>
        public void save(String summary)
        {
            if (this.statement.id == null)
            {
                throw new InvalidOperationException("The statement has no Id. Please save the statement containing it first.");
            }
            JsonObject obj = new JsonObject();
            foreach (KeyValuePair<String, Dictionary<String, Snak>> pair in this.snaks)
            {
                JsonArray array = new JsonArray();
                foreach(KeyValuePair<String, Snak> p in pair.Value)
                {
                    array.add(p.Value.Encode());
                }
                obj.add(pair.Key, array);
            }
            JsonObject result = this.statement.entity.api.setReference(this.statement.id, obj, this.hash, this.statement.entity.lastRevisionId, summary);
            this.updateDataFromResult(result);
        }

        /// <summary>
        /// Updates instance from API call result.
        /// </summary>
        /// <param name="result">Json result.</param>
        protected void updateDataFromResult(JsonObject result)
        {
            if ( result == null )
                throw new ArgumentNullException("result");

            if (result.get("reference") != null)
            {
                this.fillData(result.get("reference").asObject());
            }
            this.statement.entity.updateLastRevisionIdFromResult(result);
        }

        /// <summary>
        /// Delete the reference and save the reference which contained it.
        /// </summary>
        /// <param name="summary">The edit summary.</param>
        /// <exception cref="InvalidOperationException">Statement has no id because not saved yet.</exception>
        public void deleteAndSave(String summary)
        {
            if (this.statement.id == null)
            {
                throw new InvalidOperationException("The statement has no Id. Please save the statement containing it first.");
            }
            if (this.hash != null)
            {
                this.statement.entity.api.removeReferences(this.statement.id, new String[] { this.hash }, this.statement.entity.lastRevisionId, summary);
            }
            this.statement.removeReference(this);
        }
    }
}
