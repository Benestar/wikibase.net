using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// An item.
    /// </summary>
    public class Item : Entity
    {
        private Dictionary<String, String> sitelinks = new Dictionary<String, String>();

        #region Json names

        /// <summary>
        /// The name of the <see cref="sitelinks"/> property in the serialized json object.
        /// </summary>
        private const String SiteLinksJsonName = "sitelinks";

        /// <summary>
        /// The name of the site property of a sitelink in the serialized json object.
        /// </summary>
        private const String SiteLinksSiteJsonName = "site";

        /// <summary>
        /// The name of the title property of a sitelink in the serialized json object.
        /// </summary>
        private const String SiteLinksTitleJsonName = "title";

        /// <summary>
        /// The name of the bagdes property of a sitelink in the serialized json object.
        /// </summary>
        private const String SiteLinksBadgesJsonName = "badges";

        #endregion Json names

        /// <summary>
        /// Creates a new instance of <see cref="Item"/>.
        /// </summary>
        /// <param name="api">The api.</param>
        public Item(WikibaseApi api)
            : base(api)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Item"/> and fill it with <paramref name="data"/>.
        /// </summary>
        /// <param name="api">The api.</param>
        /// <param name="data">Json object to be parsed and added.</param>
        internal Item(WikibaseApi api, JsonObject data)
            : base(api, data)
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
            if ( data.get(SiteLinksJsonName) != null )
            {
                this.sitelinks.Clear();
                foreach ( JsonObject.Member member in data.get(SiteLinksJsonName).asObject() )
                {
                    JsonObject obj = member.value.asObject();
                    this.sitelinks.Add(obj.get(SiteLinksSiteJsonName).asString(), obj.get(SiteLinksTitleJsonName).asString());
                    // ToDo: parse badges
                }
            }
        }

        /// <summary>
        /// Get all sitelinks.
        /// </summary>
        /// <returns>The sitelinks.</returns>
        /// <remarks>Key is the project name, value the page name. To modify the sitelinks, don't modify this dictionary, but use
        /// <see cref="setSitelink"/> and <see cref="removeSitelink"/>.</remarks>
        public Dictionary<String, String> getSitelinks()
        {
            return new Dictionary<String, String>(sitelinks);
        }

        /// <summary>
        /// Get the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site</param>
        /// <returns></returns>
        public String getSitelink(String site)
        {
            return sitelinks[site];
        }

        /// <summary>
        /// Set the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="title">The sitelink.</param>
        public void setSitelink(String site, String title)
        {
            this.sitelinks[site] = title;
            if ( this.changes.get(SiteLinksJsonName) == null )
            {
                this.changes.set(SiteLinksJsonName, new JsonObject());
            }
            this.changes.get(SiteLinksJsonName).asObject().set(
                site,
                new JsonObject()
                    .add(SiteLinksSiteJsonName, site)
                    .add(SiteLinksTitleJsonName, title)
            );
        }

        /// <summary>
        /// Remove the sitelink for the given site.
        /// </summary>
        /// <param name="site">The site</param>
        /// <returns><c>true</c> if the sitelink was removed successfully, <c>false</c> otherwise.</returns>
        public Boolean removeSitelink(String site)
        {
            if ( sitelinks.Remove(site) )
            {
                if ( this.changes.get(SiteLinksJsonName) == null )
                {
                    this.changes.set(SiteLinksJsonName, new JsonObject());
                }
                this.changes.get(SiteLinksJsonName).asObject().set(
                    site,
                    new JsonObject()
                        .add(SiteLinksSiteJsonName, site)
                        .add(SiteLinksTitleJsonName, "")
                );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the type identifier of the type at server side.
        /// </summary>
        /// <returns>The type identifier.</returns>
        protected override String getType()
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
            return (Statement)Claim.newFromSnak(this, snak, "statement");
        }
    }
}