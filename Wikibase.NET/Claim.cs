using System;
using System.Collections.Generic;
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
        /// <summary>
        /// The entity
        /// </summary>
        public Entity entity { get; private set; }

        /// <summary>
        /// The id
        /// </summary>
        public string id { get; private set; }

        /// <summary>
        /// The id used internally
        /// </summary>
        public string internalId { get; private set; }

        private Snak mMainSnak;

        /// <summary>
        /// The main snak
        /// </summary>
        public Snak mainSnak {
            get
            {
                return mMainSnak;
            }
            set
            {
                if (!this.mMainSnak.propertyId.Equals(value.propertyId))
                {
                    throw new ArgumentException("Different property id");
                }
                this.mMainSnak = value;
                this.changes.set("mainsnak", value.toArray());
            }
        }

        private JsonObject changes = new JsonObject();

        internal Claim(Entity entity, JsonObject data)
        {
            this.entity = entity;
            this.fillData(data);
        }

        protected virtual void fillData(JsonObject data)
        {
            if (data.get("mainsnak") != null)
            {
                this.mMainSnak = Snak.newFromArray(data.get("mainsnak").asObject());
            }
            if (data.get("id") != null)
            {
                this.id = data.get("id").asString();
            }
            if (this.internalId == null)
            {
                if (this.id != null)
                {
                    this.internalId = this.id;
                }
                else
                {
                    this.internalId = "" + Environment.TickCount + this.mMainSnak.propertyId + this.mMainSnak.dataValue;
                }
            }
        }

        internal static Claim newFromArray(Entity entity, JsonObject data)
        {
            if (data.get("type") != null)
            {
                switch (data.get("type").asString())
                {
                    case "statement":
                        return new Statement(entity, data);
                    default:
                        return new Claim(entity, data);
                }
            }
            throw new Exception("Unknown type");
        }

        public static Claim newFromSnak(Entity entity, Snak snak, string type)
        {
            Claim claim = newFromArray(
                entity,
                new JsonObject()
                    .add("mainsnak", snak.toArray())
                    .add("type", type)
            );
            claim.changes = new JsonObject()
                .add("mainsnak", snak.toArray());
            entity.addClaim(claim);
            return claim;
        }

        public void save(string summary)
        {
            if (!this.changes.isEmpty())
            {
                if (this.changes.get("mainsnak") != null)
                {
                    JsonObject change = this.changes.get("mainsnak").asObject();
                    if (change.get("snaktype") == null || change.get("property") == null)
                    {
                        throw new Exception("The main snak does not have required data");
                    }
                    DataValue value = change.get("datavalue") == null ? null : DataValueFactory.newFromArray(change.get("datavalue").asObject().get("value").asObject());
                    JsonObject result;
                    if (this.id == null)
                    {
                        result = this.entity.api.createClaim(this.entity.id.getPrefixedId(), change.get("snaktype").asString(), change.get("property").asString(), value, this.entity.lastRevisionId, summary);
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

        protected void updateDataFromResult(JsonObject result)
        {
            if (result.get("claim") != null)
            {
                this.fillData(result.get("claim").asObject());
            }
            this.entity.updateLastRevisionIdFromResult(result);
        }

        public void deleteAndSave(string summary)
        {
            if (this.id != null)
            {
                this.entity.api.removeClaims(new string[] { this.id }, this.entity.lastRevisionId, summary);
            }
            this.entity.removeClaim(this);
        }
    }
}
