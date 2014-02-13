using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MinimalJson;
using Wikibase.DataValues;

namespace Wikibase
{
    /// <summary>
    /// A claim
    /// </summary>
    public class Claim
    {
        // TODO: Changes of qualifiers

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entitiy.</value>
        public Entity entity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        /// <remarks>Consists of the property id plus an internal identifier. Is <c>null</c> if not saved to server yet.</remarks>
        public String id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the id used internally.
        /// </summary>
        /// <value>The internally used id.</value>
        /// <remarks>Consists of the property id plus an internal identifier. It is equal to <see cref="id"/> if the claim was parsed from server results.</remarks>
        public String internalId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of qualifiers assigned to the statement.
        /// </summary>
        /// <value>Collection of qualifiers.</value>
        public ObservableCollection<Qualifier> Qualifiers
        {
            get;
            private set;
        }

        private Snak mMainSnak;

        /// <summary>
        /// The main snak
        /// </summary>
        public Snak mainSnak
        {
            get
            {
                return mMainSnak;
            }
            set
            {
                if ( value == null )
                    throw new ArgumentNullException("value");

                if ( !this.mMainSnak.PropertyId.Equals(value.PropertyId) )
                {
                    throw new ArgumentException("Different property id");
                }
                this.mMainSnak = value;
                this.changes.set("mainsnak", value.Encode());
            }
        }

        private JsonObject changes = new JsonObject();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="entity">Entity to which the claim belongs.</param>
        /// <param name="data">JSon data to be parsed.</param>
        internal Claim(Entity entity, JsonObject data)
        {
            Qualifiers = new ObservableCollection<Qualifier>();
            this.entity = entity;
            this.FillData(data);
        }

        /// <summary>
        /// Parses the <paramref name="data"/> and adds the results to this instance.
        /// </summary>
        /// <param name="data"><see cref="JsonObject"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        protected virtual void FillData(JsonObject data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            if ( data.get("mainsnak") != null )
            {
                this.mMainSnak = new Snak(data.get("mainsnak").asObject());
            }
            if ( data.get("id") != null )
            {
                this.id = data.get("id").asString();
            }
            if ( data.get("qualifiers") != null )
            {
                var qualifiers = data.get("qualifiers").asObject();

                foreach ( var entry in qualifiers.names() )
                {
                    var json = qualifiers.get(entry).asArray();
                    foreach ( var value in json )
                    {
                        var parsedQualifier = new Qualifier(value as JsonObject);
                        Qualifiers.Add(parsedQualifier);
                    }
                }
            }
            if ( this.internalId == null )
            {
                if ( this.id != null )
                {
                    this.internalId = this.id;
                }
                else
                {
                    this.internalId = "" + Environment.TickCount + this.mMainSnak.PropertyId + this.mMainSnak.DataValue;
                }
            }
        }

        internal static Claim newFromArray(Entity entity, JsonObject data)
        {
            if ( entity == null )
                throw new ArgumentNullException("entity");

            if ( data.get("type") != null )
            {
                switch ( data.get("type").asString() )
                {
                    case "statement":
                        return new Statement(entity, data);
                    default:
                        return new Claim(entity, data);
                }
            }
            throw new ArgumentException("Unknown type in data", "data");
        }

        /// <summary>
        /// Create a claim from a <see cref="Snak"/>.
        /// </summary>
        /// <param name="entity">Entity to which the claim should be added.</param>
        /// <param name="snak">Snak to be parsed.</param>
        /// <param name="type">Type of snak.</param>
        /// <returns>Newly created claim.</returns>
        public static Claim newFromSnak(Entity entity, Snak snak, String type)
        {
            if ( entity == null )
                throw new ArgumentNullException("entity");
            if ( snak == null )
                throw new ArgumentNullException("snak");

            Claim claim = newFromArray(
                entity,
                new JsonObject()
                    .add("mainsnak", snak.Encode())
                    .add("type", type)
            );
            claim.changes = new JsonObject()
                .add("mainsnak", snak.Encode());
            entity.addClaim(claim);
            return claim;
        }

        /// <summary>
        /// Saves the claim to the server.
        /// </summary>
        /// <param name="summary">Edit summary.</param>
        public void save(String summary)
        {
            if ( !this.changes.isEmpty() )
            {
                if ( this.changes.get("mainsnak") != null )
                {
                    JsonObject change = this.changes.get("mainsnak").asObject();
                    if ( change.get("snaktype") == null || change.get("property") == null )
                    {
                        throw new InvalidOperationException("The main snak does not have required data");
                    }
                    DataValue value = change.get("datavalue") == null ? null : DataValueFactory.CreateFromJsonObject(change.get("datavalue").asObject());
                    JsonObject result;
                    if ( this.id == null )
                    {
                        result = this.entity.api.createClaim(this.entity.id.PrefixedId, change.get("snaktype").asString(), change.get("property").asString(), value, this.entity.lastRevisionId, summary);
                    }
                    else
                    {
                        result = this.entity.api.setClaimValue(this.id, change.get("snaktype").asString(), value, this.entity.lastRevisionId, summary);
                    }
                    this.updateDataFromResult(result);
                    this.changes.removeAt("mainsnak");
                }
            }
        }

        /// <summary>
        /// Updates instance from API call result.
        /// </summary>
        /// <param name="result">Json result.</param>
        protected void updateDataFromResult(JsonObject result)
        {
            if ( result == null )
                throw new ArgumentNullException("result");

            if ( result.get("claim") != null )
            {
                this.FillData(result.get("claim").asObject());
            }
            this.entity.updateLastRevisionIdFromResult(result);
        }

        /// <summary>
        /// Deletes the claim both within its <see cref="entity"/> as well as on the server.
        /// </summary>
        /// <param name="summary">The edit summary.</param>
        public void deleteAndSave(String summary)
        {
            if ( this.id != null )
            {
                this.entity.api.removeClaims(new string[] { this.id }, this.entity.lastRevisionId, summary);
            }
            this.entity.removeClaim(this);
        }

        /// <summary>
        /// Checks whether the claim is about a given property.
        /// </summary>
        /// <param name="value">Property identifier string.</param>
        /// <returns><c>true</c> if is about the property, <c>false</c> otherwise.</returns>
        public Boolean IsAboutProperty(String value)
        {
            var property = new EntityId(value);
            return property.Equals(mainSnak.PropertyId);
        }
    }
}