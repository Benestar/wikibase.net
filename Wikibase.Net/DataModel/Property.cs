using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class Property : EntityDocument, FingerprintHolder, StatementListHolder
    {
        public EntityId Id { get; private set; }

        public PropertyId PropertyId { get { return (PropertyId)Id; } }

        public Fingerprint Fingerprint { get; private set; }

        public List<Statement> Statements { get; private set; }

        public bool Empty
        {
            get
            {
                return Fingerprint.Empty && Statements.Count == 0;
            }
        }

        public Property(PropertyId id = null, Fingerprint fingerprint = null, List<Statement> statements = null)
        {
            Id = id;
            Fingerprint = fingerprint == null ? new Fingerprint() : fingerprint;
            Statements = statements == null ? new List<Statement>() : statements;
        }
    }
}
