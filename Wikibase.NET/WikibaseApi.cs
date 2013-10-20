using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;
using System.Diagnostics;
using System.Threading;
using Wikibase.DataValues;

namespace Wikibase
{
    /// <summary>
    /// Base class for the Wikibase API.
    /// </summary>
    public class WikibaseApi : Api
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wiki">he base url of the wiki like "https://www.wikidata.org"</param>
        public WikibaseApi(string wiki) : base(wiki) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wiki">The base url of the wiki like "https://www.wikidata.org"</param>
        /// <param name="userAgent">The user agent</param>
        public WikibaseApi(string wiki, string userAgent) : base(wiki, userAgent) { }

        /// <summary>
        /// Get the data for the entities in the given languages from the provided ids.
        /// </summary>
        /// <param name="ids">The ids</param>
        /// <param name="languages">The languages</param>
        /// <returns>The list of entities</returns>
        internal Entity[] getEntitiesFromIds(string[] ids, string[] languages)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbgetentities" },
                { "ids", string.Join("|", ids ) }
            };
            if(languages != null)
            {
                parameters["languages"] = string.Join("|", languages);
            }
            JsonObject result = this.get(parameters);
            return parseGetEntitiesApiResponse(result);
        }

        /// <summary>
        /// Get the data for the entities in the given languages from the provided sites and titles.
        /// </summary>
        /// <param name="sites">The sites</param>
        /// <param name="titles">The titles</param>
        /// <param name="languages">The languaes</param>
        /// <returns>The list of entities</returns>
        internal Entity[] getEntitesFromSitelinks(string[] sites, string[] titles, string[] languages)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbgetentities" },
                { "sites", string.Join("|", sites ) },
                { "titles", string.Join("|", titles ) }
            };
            if (languages != null)
            {
                parameters["languages"] = string.Join("|", languages);
            }
            JsonObject result = this.get(parameters);
            return parseGetEntitiesApiResponse(result);
        }

        /// <summary>
        /// Create a list of entities form an api response.
        /// </summary>
        /// <param name="result">The result of the api request</param>
        /// <returns>The list of entities</returns>
        protected Entity[] parseGetEntitiesApiResponse(JsonObject result)
        {
            List<Entity> entities = new List<Entity>();
            if (result.get("entities") != null)
            {
                foreach (JsonObject.Member member in result.get("entities").asObject())
                {
                    if (member.value.asObject().get("missing") == null)
                    {
                        entities.Add(Entity.newFromArray(this, member.value.asObject()));
                    }
                }
            }
            return entities.ToArray();
        }

        /// <summary>
        /// Edit an entity.
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <param name="data">The serialized data of the entity</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject editEntity(string id, JsonObject data, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbeditentity" }
            };
            Dictionary<string, string> postFields = new Dictionary<string, string>()
            {
                { "data", data.ToString() },
                { "id", id }
            };
            return this.editAction(parameters, postFields, baseRevisionId, summary);
        }

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="type">The type of the entity</param>
        /// <param name="data">The serialized data of the entity</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject createEntity(string type, JsonObject data, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbeditentity" }
            };
            Dictionary<string, string> postFields = new Dictionary<string, string>()
            {
                { "data", data.ToString() },
                { "new", type }
            };
            return this.editAction(parameters, postFields, baseRevisionId, summary);
        }

        /// <summary>
        /// Create a claim.
        /// </summary>
        /// <param name="entity">The id of the entity you are adding the claim to</param>
        /// <param name="snakType">The type of the snak</param>
        /// <param name="property">The id of the snak property</param>
        /// <param name="value">The value of the snak when creating a claim with a snak that has a value</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject createClaim(string entity, string snakType, string property, DataValue value, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbcreateclaim" },
                { "entity", entity },
                { "snaktype", snakType },
                { "proeprty", property }
            };
            if (value != null)
            {
                parameters["value"] = value.encode().ToString();
            }
            return this.editAction(parameters, new Dictionary<string, string>(), baseRevisionId, summary);
        }

        /// <summary>
        /// Set a claim value.
        /// </summary>
        /// <param name="claim">GUID identifying the claim</param>
        /// <param name="snakType">The type of the snak</param>
        /// <param name="value">The value of the snak when creating a claim with a snak that has a value</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject setClaimValue(string claim, string snakType, DataValue value, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbsetclaimvalue" },
                { "claim", claim },
                { "snaktype", snakType }
            };
            if (value != null)
            {
                parameters["value"] = value.encode().ToString();
            }
            return this.editAction(parameters, new Dictionary<string, string>(), baseRevisionId, summary);
        }

        /// <summary>
        /// Remove the claims.
        /// </summary>
        /// <param name="claims">The claims</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject removeClaims(string[] claims, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbremoveclaims" },
                { "claim", string.Join("|", claims) }
            };
            return this.editAction(parameters, new Dictionary<string, string>(), baseRevisionId, summary);
        }

        /// <summary>
        /// Set a reference.
        /// </summary>
        /// <param name="statement">GUID identifying the statement</param>
        /// <param name="snaks">The snaks to set the reference to. Array with property ids pointing to arrays containing the snaks for that property</param>
        /// <param name="reference">A hash of the reference that should be updated. When not provided, a new reference is created</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject setReference(string statement, JsonObject snaks, string reference, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbsetreference" },
                { "statement", statement },
                { "snaks", snaks.ToString() }
            };
            if (reference != null)
            {
                parameters["reference"] = reference;
            }
            return this.editAction(parameters, new Dictionary<string, string>(), baseRevisionId, summary);
        }

        /// <summary>
        /// Remove the references.
        /// </summary>
        /// <param name="statement">GUID identifying the statement</param>
        /// <param name="references">The hashes of the references that should be removed</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        internal JsonObject removeReferences(string statement, string[] references, int baseRevisionId, string summary)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "action", "wbremovereferences" },
                { "statement", statement },
                { "references", string.Join("|", references) }
            };
            return this.editAction(parameters, new Dictionary<string, string>(), baseRevisionId, summary);
        }

        /// <summary>
        /// Perform an edit action.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <param name="postFields">The post fields</param>
        /// <param name="baseRevisionId">The numeric identifier for the revision to base the modification on</param>
        /// <param name="summary">The summary for the change</param>
        /// <returns>The result</returns>
        protected JsonObject editAction(Dictionary<string, string> parameters, Dictionary<string, string> postFields, int baseRevisionId, string summary)
        {
            parameters["token"] = this.getEditToken();
            if (baseRevisionId != 0)
            {
                parameters["baserevid"] = baseRevisionId.ToString();
            }
            if (summary != null)
            {
                parameters["summary"] = summary;
            }
            if (this.botEdits)
            {
                parameters["bot"] = true.ToString();
            }
            // limit number of edits
            int time = Environment.TickCount;
            if (this.lastEditTimestamp > 0 && (time - this.lastEditTimestamp) < this.editLaps)
            {
                int wait = this.lastEditTimestamp + this.editLaps - time;
                Console.WriteLine("Wait for {0} seconds...", wait / 1000);
                Thread.Sleep(wait);
            }
            this.lastEditTimestamp = Environment.TickCount;
            return this.post(parameters, postFields);
        }
    }
}
