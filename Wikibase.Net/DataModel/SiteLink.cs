using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public struct SiteLink
    {
        public readonly string SiteId;

        public readonly string PageName;

        public readonly HashSet<ItemId> Badges;

        public SiteLink(string siteId, string pageName, HashSet<ItemId> badges)
        {
            if (siteId == null || pageName == null)
            {
                throw new ArgumentNullException("The site id and page name must not be null");
            }

            SiteId = siteId;
            PageName = pageName;
            Badges = badges == null ? new HashSet<ItemId>() : badges;
        }

        public SiteLink(string siteId, string pageName)
            : this(siteId, pageName, null)
        {
        }
    }
}
