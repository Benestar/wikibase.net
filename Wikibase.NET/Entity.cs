using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// An entity
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// The entity id
        /// </summary>
        public EntityId id
        {
            get;
            private set;
        }

        /// <summary>
        /// The last revision id
        /// </summary>
        public int lastRevisionId
        {
            get;
            set;
        }

        /// <summary>
        /// The api
        /// </summary>
        public WikibaseApi api
        {
            get;
            private set;
        }

        /// <summary>
        /// Labels, the actual name. Key is the language editifier, value the label.
        /// </summary>
        private Dictionary<String, String> labels = new Dictionary<String, String>();

        /// <summary>
        /// Descriptions, to explain the item. Key is the language editifier, value the description.
        /// </summary>
        private Dictionary<String, String> descriptions = new Dictionary<String, String>();

        /// <summary>
        /// Aliases. Key is the language editifier, value a list of aliases in the given language.
        /// </summary>
        private Dictionary<String, List<String>> aliases = new Dictionary<String, List<String>>();

        /// <summary>
        /// Claims. Key is the property Id, value a dictionary with the claims internal id as the key and the actual claim as the value.
        /// </summary>
        private Dictionary<String, Dictionary<String, Claim>> claims = new Dictionary<String, Dictionary<String, Claim>>();

        /// <summary>
        /// Changes cache.
        /// </summary>
        protected JsonObject changes = new JsonObject();

        /// <summary>
        /// Constructor creating a blank entity instance.
        /// </summary>
        /// <param name="api">The api.</param>
        public Entity(WikibaseApi api)
            : this(api, new JsonObject())
        {
        }

        /// <summary>
        /// Constructor creating an entitiy from a Json object.
        /// </summary>
        /// <param name="api">The api</param>
        /// <param name="data">The json object to be parsed.</param>
        internal Entity(WikibaseApi api, JsonObject data)
        {
            this.api = api;
            this.fillData(data);
        }

        /// <summary>
        /// Parses the <paramref name="data"/> and adds the results to this instance.
        /// </summary>
        /// <param name="data"><see cref="JsonObject"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        protected virtual void fillData(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            if ( data.get("id") != null )
            {
                this.id = EntityId.newFromPrefixedId(data.get("id").asString());
            }
            if ( data.get("lastrevid") != null )
            {
                this.lastRevisionId = data.get("lastrevid").asInt();
            }
            if ( data.get("labels") != null )
            {
                this.labels = new Dictionary<string, string>();
                foreach ( JsonObject.Member member in data.get("labels").asObject() )
                {
                    JsonObject obj = member.value.asObject();
                    this.labels.Add(obj.get("language").asString(), obj.get("value").asString());
                }
            }
            if ( data.get("descriptions") != null )
            {
                this.descriptions = new Dictionary<string, string>();
                foreach ( JsonObject.Member member in data.get("descriptions").asObject() )
                {
                    JsonObject obj = member.value.asObject();
                    this.descriptions.Add(obj.get("language").asString(), obj.get("value").asString());
                }
            }
            JsonValue returnedAliases = data.get("aliases");
            if ( (returnedAliases != null) && (returnedAliases.isObject()) )
            {
                // strange - after save an empty array is returned, whereas by a normal get the fully alias list is returned
                this.aliases = new Dictionary<string, List<string>>();
                foreach ( JsonObject.Member member in returnedAliases.asObject() )
                {
                    List<string> list = new List<string>();
                    foreach ( JsonValue value in member.value.asArray() )
                    {
                        list.Add(value.asObject().get("value").asString());
                    }
                    this.aliases.Add(member.name, list);
                }
            }
            if ( data.get("claims") != null )
            {
                this.claims = new Dictionary<string, Dictionary<string, Claim>>();
                foreach ( JsonObject.Member member in data.get("claims").asObject() )
                {
                    Dictionary<string, Claim> list = new Dictionary<string, Claim>();
                    foreach ( JsonValue value in member.value.asArray() )
                    {
                        Claim claim = Claim.newFromArray(this, value.asObject());
                        list.Add(claim.internalId, claim);
                    }
                    this.claims.Add(member.name, list);
                }
            }
        }

        internal static Entity newFromArray(WikibaseApi api, JsonObject data)
        {
            if ( data.get("type") != null )
            {
                switch ( data.get("type").asString() )
                {
                    case "item":
                        return new Item(api, data);
                    case "property":
                        return new Property(api, data);
                }
            }
            throw new Exception("Unknown type");
        }

        /// <summary>
        /// Get all labels.
        /// </summary>
        /// <returns>The labels</returns>
        /// <remarks>Key is the language, value the label.</remarks>
        public Dictionary<String, String> getLabels()
        {
            return new Dictionary<String, String>(labels);
        }

        /// <summary>
        /// Get the label for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns>The label.</returns>
        /// <exception cref="ArgumentException"><paramref name="lang"/> is empty string or <c>null</c>.</exception>
        public String getLabel(String lang)
        {
            if ( String.IsNullOrWhiteSpace(lang) )
                throw new ArgumentException("empty language");
            return labels.ContainsKey(lang) ? labels[lang] : null;
        }

        /// <summary>
        /// Set the label for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <param name="value">The label.</param>
        /// <exception cref="ArgumentException"><paramref name="lang"/> or <paramref name="value"/> is empty string or <c>null</c>.</exception>
        public void setLabel(String lang, String value)
        {
            if ( String.IsNullOrWhiteSpace(value) )
                throw new ArgumentException("empty description");
            if ( String.IsNullOrWhiteSpace(lang) )
                throw new ArgumentException("empty language");

            if ( getLabel(lang) != value )
            {
                this.labels[lang] = value;
                if ( this.changes.get("labels") == null )
                {
                    this.changes.set("labels", new JsonObject());
                }
                this.changes.get("labels").asObject().set(
                    lang,
                    new JsonObject()
                        .add("language", lang)
                        .add("value", value)
                );
            }
        }

        /// <summary>
        /// Remove the label for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns><c>true</c> if the label was removed successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="lang"/> is empty string or <c>null</c>.</exception>
        public Boolean removeLabel(String lang)
        {
            if ( String.IsNullOrWhiteSpace(lang) )
                throw new ArgumentException("empty language");
            if ( this.labels.Remove(lang) )
            {
                if ( this.changes.get("labels") == null )
                {
                    this.changes.set("labels", new JsonObject());
                }
                this.changes.get("labels").asObject().set(
                    lang,
                    new JsonObject()
                        .add("language", lang)
                        .add("value", "")
                );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get all descriptions.
        /// </summary>
        /// <returns>The descriptions.</returns>
        /// <remarks>Keys is the language, value the description.</remarks>
        public Dictionary<String, String> getDescriptions()
        {
            return new Dictionary<String, String>(descriptions);
        }

        /// <summary>
        /// Get the description for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns>The description.</returns>
        /// <exception cref="ArgumentException"><paramref name="lang"/> is empty string or <c>null</c>.</exception>
        public string getDescription(String lang)
        {
            if ( String.IsNullOrWhiteSpace(lang) )
                throw new ArgumentException("empty language");
            return descriptions.ContainsKey(lang) ? descriptions[lang] : null;
        }

        /// <summary>
        /// Set the description for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <param name="value">The description.</param>
        /// <exception cref="ArgumentException"><paramref name="lang"/> or <paramref name="value"/> is empty string or <c>null</c>.</exception>
        public void setDescription(String lang, String value)
        {
            if ( String.IsNullOrWhiteSpace(value) )
                throw new ArgumentException("empty description");
            if ( String.IsNullOrWhiteSpace(lang) )
                throw new ArgumentException("empty language");

            if ( getDescription(lang) != value )
            {
                this.descriptions[lang] = value;
                if ( this.changes.get("descriptions") == null )
                {
                    this.changes.set("descriptions", new JsonObject());
                }
                this.changes.get("descriptions").asObject().set(
                    lang,
                    new JsonObject()
                        .add("language", lang)
                        .add("value", value)
                );
            }
        }

        /// <summary>
        /// Remove the description for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns><c>true</c> if the description was removed successfully, <c>false</c> otherwise.</returns>
        public bool removeDescription(String lang)
        {
            if ( this.descriptions.Remove(lang) )
            {
                if ( this.changes.get("descriptions") == null )
                {
                    this.changes.set("descriptions", new JsonObject());
                }
                this.changes.get("descriptions").asObject().set(
                    lang,
                    new JsonObject()
                        .add("language", lang)
                        .add("value", "")
                );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get all aliases.
        /// </summary>
        /// <returns>The aliases</returns>
        /// <value>Key is the language, value a list of aliases.</value>
        public Dictionary<String, List<String>> getAliases()
        {
            Dictionary<String, List<String>> copy = new Dictionary<String, List<String>>(aliases);
            foreach ( KeyValuePair<String, List<String>> pair in aliases )
            {
                copy[pair.Key] = new List<string>(pair.Value);
            }
            return copy;
        }

        /// <summary>
        /// Get the aliases for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns>The aliases, or <c>null</c> if no aliases are defined for the language.</returns>
        public List<String> getAlias(String lang)
        {
            return aliases.ContainsKey(lang) ? new List<String>(aliases[lang]) : null;
        }

        /// <summary>
        /// Add an alias for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <param name="value">The alias.</param>
        public void addAlias(String lang, String value)
        {
            if ( !this.aliases.ContainsKey(lang) )
            {
                this.aliases.Add(lang, new List<String>());
            }
            if ( !aliases[lang].Contains(value) )
            {
                this.aliases[lang].Add(value);
                if ( this.changes.get("aliases") == null )
                {
                    this.changes.set("aliases", new JsonArray());
                }
                // Override if needed an action on the same alias
                for ( int i = 0 ; i < this.changes.get("aliases").asArray().size() ; i++ )
                {
                    JsonObject obj = this.changes.get("aliases").asArray().get(i).asObject();
                    if ( obj.get("language").asString() == lang && obj.get("value").asString() == value )
                    {
                        this.changes.get("aliases").asArray().removeAt(i);
                        break;
                    }
                }
                this.changes.get("aliases").asArray().add(
                    new JsonObject()
                        .add("language", lang)
                        .add("value", value)
                        .add("add", true)
                );
            }
        }

        /// <summary>
        /// Remove the alias for the given language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <param name="value">The alias.</param>
        /// <returns><c>true</c> if the alias was removed successfully, <c>false</c> otherwise.</returns>
        public Boolean removeAlias(String lang, String value)
        {
            if ( this.aliases.ContainsKey(lang) )
            {
                if ( this.aliases[lang].Remove(value) )
                {
                    if ( this.changes.get("aliases") == null )
                    {
                        this.changes.set("aliases", new JsonArray());
                    }
                    // Override if needed an action on the same alias
                    for ( int i = 0 ; i < this.changes.get("aliases").asArray().size() ; i++ )
                    {
                        JsonObject obj = this.changes.get("aliases").asArray().get(i).asObject();
                        if ( obj.get("language").asString() == lang && obj.get("value").asString() == value )
                        {
                            this.changes.get("aliases").asArray().removeAt(i);
                            break;
                        }
                    }
                    this.changes.get("aliases").asArray().add(
                        new JsonObject()
                            .add("language", lang)
                            .add("value", value)
                            .add("remove", true)
                    );
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all claims.
        /// </summary>
        /// <returns>The claims.</returns>
        /// <remarks>Key is property id, value a dictionary in which the key is the internal id and value is the actual claim.</remarks>
        public Dictionary<String, Dictionary<String, Claim>> getClaims()
        {
            Dictionary<String, Dictionary<String, Claim>> copy = new Dictionary<String, Dictionary<String, Claim>>(claims);
            foreach ( KeyValuePair<String, Dictionary<String, Claim>> pair in claims )
            {
                copy[pair.Key] = new Dictionary<String, Claim>(pair.Value);
            }
            return copy;
        }

        /// <summary>
        /// Gets all claims.
        /// </summary>
        /// <value>All claims.</value>
        public IEnumerable<Claim> Claims
        {
            get
            {
                return claims.Values.SelectMany(x => x.Values).ToList();
            }
        }

        /// <summary>
        /// Get the claims for the given property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The claims.</returns>
        public Dictionary<String, Claim> getClaimsForProperty(String property)
        {
            return claims.ContainsKey(property) ? new Dictionary<String, Claim>(claims[property]) : null;
        }

        /// <summary>
        /// Add the claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        internal void addClaim(Claim claim)
        {
            string property = claim.mainSnak.propertyId.getPrefixedId();
            if ( !this.claims.ContainsKey(property) )
            {
                this.claims[property] = new Dictionary<string, Claim>();
            }
            this.claims[property][claim.internalId] = claim;
        }

        /// <summary>
        /// Remove the claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns><c>true</c> if the claim was removed successfully, <c>false</c> otherwise.</returns>
        internal Boolean removeClaim(Claim claim)
        {
            string property = claim.mainSnak.propertyId.getPrefixedId();
            if ( !this.claims.ContainsKey(property) )
            {
                return false;
            }
            if ( this.claims[property].Remove(claim.internalId) )
            {
                if ( this.claims[property].Count == 0 )
                {
                    this.claims.Remove(property);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save all changes.
        /// </summary>
        /// <param name="summary">The edit summary.</param>
        public void save(String summary)
        {
            if ( !this.changes.isEmpty() )
            {
                JsonObject result;
                if ( this.id == null )
                {
                    result = this.api.createEntity(this.getType(), this.changes, this.lastRevisionId, summary);
                }
                else
                {
                    result = this.api.editEntity(this.id.getPrefixedId(), this.changes, this.lastRevisionId, summary);
                }
                if ( result.get("entity") != null )
                {
                    this.fillData(result.get("entity").asObject());
                }
                this.updateLastRevisionIdFromResult(result);
                this.changes = new JsonObject();
            }
        }

        internal void updateLastRevisionIdFromResult(JsonObject result)
        {
            if ( result.get("pageinfo") != null && result.get("pageinfo").asObject().get("lastrevid") != null )
            {
                this.lastRevisionId = result.get("pageinfo").asObject().get("lastrevid").asInt();
            }
        }

        /// <summary>
        /// Gets the type identifier of the type at server side.
        /// </summary>
        /// <returns>The type identifier.</returns>
        protected abstract String getType();
    }
}