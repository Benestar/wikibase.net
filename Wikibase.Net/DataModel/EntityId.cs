using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public abstract class EntityId
    {
        abstract protected string regex { get; }

        public string Serialization { get; private set; }

        public EntityId(string serialization)
        {
            if (!Regex.IsMatch(serialization, regex))
            {
                throw new ArgumentException("The serialization has to match the format \"" + regex + "\"");
            }

            this.Serialization = serialization;
        }

        public override int GetHashCode()
        {
            return Serialization.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            EntityId entityId = obj as EntityId;
            if ((System.Object)entityId == null)
            {
                return false;
            }

            return entityId.Serialization == Serialization;
        }
    }
}
