using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MinimalJson;

namespace Wikibase
{
    /// <summary>
    /// Base api class
    /// </summary>
    public class Api
    {
        private const String ApiName = "Wikibase.NET";
        private const String ApiVersion = "0.1";

        private Http http;
        private String wiki;
        private String editToken;

        /// <summary>
        /// Gets the sets the time stamp of the last API action.
        /// </summary>
        /// <value>The time stamp of the last API action.</value>
        protected Int32 lastEditTimestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the sets the time lap between two consecutive API actions in milliseconds.
        /// </summary>
        /// <value>The time lap in milliseconds.</value>
        public Int32 editLaps
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if bot edits should be used.
        /// </summary>
        /// <value><c>true</c> if bot edits are used, <c>false</c> otherwise.</value>
        public Boolean botEdits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if the edits should be limited.
        /// </summary>
        /// <value><c>true</c> if the edits are limited, <c>false</c> otherwise.</value>
        public Boolean editlimit
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wiki">The base url of the wiki like "http://www.wikidata.org".</param>
        public Api(String wiki)
            : this(wiki, ApiName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wiki">The base url of the wiki like "https://www.wikidata.org".</param>
        /// <param name="userAgent">The user agent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userAgent"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="wiki"/> is empty or <c>null</c>.</exception>
        public Api(String wiki, String userAgent)
        {
            if ( String.IsNullOrWhiteSpace(wiki) )
                throw new ArgumentException("Inavlid base url", "wiki");
            if ( userAgent == null )
                throw new ArgumentNullException("userAgent");

            this.wiki = wiki;
            this.http = new Http(String.Format(CultureInfo.InvariantCulture, "{0} {1}/{2}", userAgent.Trim(), ApiName, ApiVersion));
        }

        /// <summary>
        /// Perform a http get request to the api.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <c>null</c>.</exception>
        public JsonObject get(Dictionary<String, String> parameters)
        {
            if ( parameters == null )
                throw new ArgumentNullException("parameters");

            parameters["format"] = "json";
            String url = this.wiki + "/w/api.php?" + http.buildQuery(parameters);
            String response = http.get(url);
            JsonValue result = JsonValue.readFrom(response);
            if ( !result.isObject() )
            {
                return null;
            }
            JsonObject obj = result.asObject();
            if ( obj.get("error") != null )
            {
                throw new ApiException(obj.get("error").asObject().get("info").asString());
            }
            return obj;
        }

        /// <summary>
        /// Perform a http post request to the api.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="postFields">The post fields.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> or <paramref name="postFields"/> is <c>null</c>.</exception>
        public JsonObject post(Dictionary<String, String> parameters, Dictionary<String, String> postFields)
        {
            if ( parameters == null )
                throw new ArgumentNullException("parameters");
            if ( postFields == null )
                throw new ArgumentNullException("postFields");

            parameters["format"] = "json";
            String url = this.wiki + "/w/api.php?" + http.buildQuery(parameters);
            String response = http.post(url, postFields);
            JsonObject result = JsonObject.readFrom(response);
            if ( result.get("error") != null )
            {
                throw new ApiException(result.get("error").asObject().get("info").asString());
            }
            return result;
        }

        /// <summary>
        /// Get the continuation parameter of a query.
        /// </summary>
        /// <param name="result">The result of the query.</param>
        /// <returns>An array containing the continuation parameter key at 0 and the continuation parameter value at 1.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="result"/> is <c>null</c>.</exception>
        public String[] getContinueParam(JsonObject result)
        {
            if ( result == null )
                throw new ArgumentNullException("result");

            if ( result.get("query-continue") != null )
            {
                List<String> keys = (List<String>)result.get("query-continue").asObject().names();
                List<String> keys2 = (List<String>)result.get("query-continue").asObject().get(keys[0]).asObject().names();
                return new String[] { keys2[0], result.get("query-continue").asObject().get(keys[0]).asObject().get(keys2[0]).asString() };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Do login.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if the user is logged in successfully, <c>false</c> otherwise.</returns>
        public Boolean login(String userName, String password)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>()
            {
                { "action", "login" }
            };
            Dictionary<String, String> postFields = new Dictionary<String, String>()
            {
                { "lgname", userName },
                { "lgpassword", password }
            };
            JsonObject login = this.post(parameters, postFields).get("login").asObject();
            if ( login.get("result").asString() == "NeedToken" )
            {
                postFields["lgtoken"] = login.get("token").asString();
                login = this.post(parameters, postFields).get("login").asObject();
            }
            if ( login.get("result").asString() == "Success" )
            {
                this.editToken = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Do logout.
        /// </summary>
        public void logout()
        {
            Dictionary<string, string> parameters = new Dictionary<String, String>()
            {
                { "action", "logout" }
            };
            this.get(parameters);
            this.editToken = null;
        }

        /// <summary>
        /// Return the edit token for the current user.
        /// </summary>
        /// <returns>The edit token.</returns>
        public String getEditToken()
        {
            if ( this.editToken == null )
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>()
                {
                    { "action", "query" },
                    { "prop", "info" },
                    { "intoken", "edit" },
                    { "titles", "Main Page" }
                };
                JsonObject query = this.get(parameters).get("query").asObject();
                foreach ( JsonObject.Member member in query.get("pages").asObject() )
                {
                    return member.value.asObject().get("edittoken").asString();
                }
            }
            return editToken;
        }
    }
}