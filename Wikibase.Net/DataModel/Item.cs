using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class Item : EntityDocument, FingerprintHolder, StatementListHolder
    {
        public EntityId Id { get; private set; }

        public ItemId ItemId { get { return (ItemId)Id; } }

        public Fingerprint Fingerprint { get; private set; }

        public List<Statement> Statements { get; private set; }

        public IndexedList<string, SiteLink> Sitelinks { get; private set; }

        public bool Empty
        {
            get
            {
                return Fingerprint.Empty && Statements.Count == 0 && Sitelinks.Count == 0;
            }
        }

        public Item(ItemId id = null, Fingerprint fingerprint = null, List<Statement> statements = null, List<SiteLink> sitelinks = null)
        {
            Id = id;
            Fingerprint = fingerprint == null ? new Fingerprint() : fingerprint;
            Statements = statements == null ? new List<Statement>() : statements;
            Sitelinks = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId, sitelinks);
        }
    }
}
