using System;
using System.Collections.Generic;
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

        private Dictionary<string, string> labels = new Dictionary<string, string>();

        private Dictionary<string, string> descriptions = new Dictionary<string, string>();

        private Dictionary<string, List<string>> aliases = new Dictionary<string, List<string>>();

        private Dictionary<string, Dictionary<string, Claim>> claims = new Dictionary<string, Dictionary<string, Claim>>();

        protected JsonObject changes = new JsonObject();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The api</param>
        public Entity(WikibaseApi api)
            : this(api, new JsonObject())
        {
        }

        internal Entity(WikibaseApi api, JsonObject data)
        {
            this.api = api;
            this.fillData(data);
        }

        protected virtual void fillData(JsonObject data)
        {
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
        public Dictionary<string, string> getLabels()
        {
            return new Dictionary<string, string>(labels);
        }

        /// <summary>
        /// Get the label for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <returns>The label</returns>
        public string getLabel(string lang)
        {
            return labels.ContainsKey(lang) ? labels[lang] : null;
        }

        /// <summary>
        /// Set the label for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <param name="value">The label</param>
        public void setLabel(string lang, string value)
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

        /// <summary>
        /// Remove the label for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <returns>If the label was removed successfully</returns>
        public bool removeLabel(string lang)
        {
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
        /// <returns>The descriptions</returns>
        public Dictionary<string, string> getDescriptions()
        {
            return new Dictionary<string, string>(descriptions);
        }

        /// <summary>
        /// Get the description for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <returns>The description</returns>
        public string getDescription(string lang)
        {
            return descriptions.ContainsKey(lang) ? descriptions[lang] : null;
        }

        /// <summary>
        /// Set the description for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <param name="value">The label</param>
        public void setDescription(string lang, string value)
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

        /// <summary>
        /// Remove the description for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <returns>If the description was removed successfully</returns>
        public bool removeDescription(string lang)
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
        public Dictionary<string, List<string>> getAliases()
        {
            Dictionary<string, List<string>> copy = new Dictionary<string, List<string>>(aliases);
            foreach ( KeyValuePair<string, List<string>> pair in aliases )
            {
                copy[pair.Key] = new List<string>(pair.Value);
            }
            return copy;
        }

        /// <summary>
        /// Get the aliases for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <returns>The aliases</returns>
        public List<string> getAlias(string lang)
        {
            return aliases.ContainsKey(lang) ? new List<string>(aliases[lang]) : null;
        }

        /// <summary>
        /// Add the alias for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <param name="value">The alias</param>
        public void addAlias(string lang, string value)
        {
            if ( !this.aliases.ContainsKey(lang) )
            {
                this.aliases.Add(lang, new List<string>());
            }
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

        /// <summary>
        /// Remove the alias for the given language.
        /// </summary>
        /// <param name="lang">The language</param>
        /// <param name="value">The alias</param>
        /// <returns>If the alias was removed successfully</returns>
        public bool removeAlias(string lang, string value)
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
        /// <returns>The claims</returns>
        public Dictionary<string, Dictionary<string, Claim>> getClaims()
        {
            Dictionary<string, Dictionary<string, Claim>> copy = new Dictionary<string, Dictionary<string, Claim>>(claims);
            foreach ( KeyValuePair<string, Dictionary<string, Claim>> pair in claims )
            {
                copy[pair.Key] = new Dictionary<string, Claim>(pair.Value);
            }
            return copy;
        }

        /// <summary>
        /// Get the claims for the given property.
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>The claims</returns>
        public Dictionary<string, Claim> getClaimsForProperty(string property)
        {
            return claims.ContainsKey(property) ? new Dictionary<string, Claim>(claims[property]) : null;
        }

        /// <summary>
        /// Add the claim.
        /// </summary>
        /// <param name="claim">The claim</param>
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
        /// <param name="claim">The claim</param>
        /// <returns>If the claim was removed successfully</returns>
        internal bool removeClaim(Claim claim)
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
        /// <param name="summary">The edit summary</param>
        public void save(string summary)
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

        protected abstract string getType();
    }
}