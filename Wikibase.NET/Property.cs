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
        public string datatype
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor creating a blank property.
        /// </summary>
        /// <param name="api">The api</param>
        public Property(WikibaseApi api)
            : base(api)
        {
        }

        /// <summary>
        /// Constructor creating a property from a Json object.
        /// </summary>
        /// <param name="api">The api</param>
        /// <param name="data">The json object to be parsed.</param>
        internal Property(WikibaseApi api, JsonObject data)
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
            if ( data.get("datatype") != null )
            {
                this.datatype = data.get("datatype").asString();
            }
        }

        /// <summary>
        /// Gets the type identifier of the type at server side.
        /// </summary>
        /// <returns>The type identifier.</returns>
        protected override string getType()
        {
            return "property";
        }
    }
}