using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// An item
    /// </summary>
    public class Item : Entity
    {
        private Dictionary<string, string> sitelinks = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The api</param>
        public Item(WikibaseApi api)
            : base(api)
        {
        }

        internal Item(WikibaseApi api, JsonObject data)
            : base(api, data)
        {
        }

        protected override void fillData(JsonObject data)
        {
            base.fillData(data);
            if ( data.get("sitelinks") != null )
            {
                foreach ( JsonObject.Member member in data.get("sitelinks").asObject() )
                {
                    JsonObject obj = member.value.asObject();
                    this.sitelinks.Add(obj.get("site").asString(), obj.get("title").asString());
                }
            }
        }

        /// <summary>
        /// Get all sitelinks.
        /// </summary>
        /// <returns>The sitelinks</returns>
        public Dictionary<string, string> getSitelinks()
        {
            return new Dictionary<string, string>(sitelinks);
        }

        /// <summary>
        /// Get the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site</param>
        /// <returns></returns>
        public string getSitelink(string site)
        {
            return sitelinks[site];
        }

        /// <summary>
        /// Set the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site</param>
        /// <param name="sitelink">The sitelink</param>
        public void setSitelink(string site, string title)
        {
            this.sitelinks[site] = title;
            if (this.changes.get("sitelinks") == null)
            {
                this.changes.set("sitelinks", new JsonObject());
            }
            this.changes.get("sitelinks").asObject().set(
                site,
                new JsonObject()
                    .add("site", site)
                    .add("title", title)
            );
        }

        /// <summary>
        /// Remove the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site</param>
        /// <returns>If the sitelink was removed successfully</returns>
        public bool removeSitelink(string site)
        {
            if (sitelinks.Remove(site))
            {
                if (this.changes.get("sitelinks") == null)
                {
                    this.changes.set("sitelinks", new JsonObject());
                }
                this.changes.get("sitelinks").asObject().set(
                    site,
                    new JsonObject()
                        .add("site", site)
                        .add("title", "")
                );
                return true;
            }
            return false;
        }

        protected override string getType()
        {
            return "item";
        }

        /// <summary>
        /// Create a new statement in this item for the provided snak.
        /// </summary>
        /// <param name="snak">The snak</param>
        /// <returns>The statement</returns>
        public Statement createStatementForSnak(Snak snak)
        {
            return (Statement) Claim.newFromSnak(this, snak, "statement");
        }
    }
}
