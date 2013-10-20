using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// A property
    /// </summary>
    public class Property : Entity
    {
        /// <summary>
        /// The data type
        /// </summary>
        public string datatype { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The api</param>
        public Property(WikibaseApi api) : base(api) { }

        internal Property(WikibaseApi api, JsonObject data) : base(api, data) { }

        protected override void fillData(JsonObject data)
        {
            base.fillData(data);
            if (data.get("datatype") != null)
            {
                this.datatype = data.get("datatype").asString();
            }
        }

        protected override string getType()
        {
            return "property";
        }
    }
}
