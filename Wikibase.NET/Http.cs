using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Wikibase
{
    /// <summary>
    /// Http related code.
    /// </summary>
    internal class Http
    {
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public String UserAgent
        {
            get;
            set;
        }

        private CookieContainer cookies = new CookieContainer();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userAgent">The user agent</param>
        public Http(String userAgent)
        {
            this.UserAgent = userAgent;
        }

        /// <summary>
        /// Performs a http get request.
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The response</returns>
        public String get(String url)
        {
            return this.post(url, null);
        }

        /// <summary>
        /// Performs a http post request.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="postFields">The post fields.</param>
        /// <returns>The response.</returns>
        public String post(String url, Dictionary<String, String> postFields)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.UserAgent = this.UserAgent;
            request.ContentType = "application/x-www-form-urlencoded";

            if ( this.cookies.Count == 0 )
                request.CookieContainer = new CookieContainer();
            else
                request.CookieContainer = this.cookies;

            if ( postFields != null )
            {
                request.Method = "POST";
                byte[] postBytes = Encoding.UTF8.GetBytes(this.buildQuery(postFields));
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            foreach ( Cookie cookie in response.Cookies )
            {
                this.cookies.Add(cookie);
            }

            Stream respStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(respStream);
            string respStr = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return respStr;
        }

        /// <summary>
        /// Builds a http query string.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns>The query string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fields"/> is <c>null</c>.</exception>
        public String buildQuery(Dictionary<String, String> fields)
        {
            if ( fields == null )
                throw new ArgumentNullException("fields");

            String query = String.Empty;
            foreach ( KeyValuePair<String, String> field in fields )
            {
                query += HttpUtility.UrlEncode(field.Key) + "=" + HttpUtility.UrlEncode(field.Value) + "&";
            }
            if ( !String.IsNullOrEmpty(query) )
            {
                query = query.Remove(query.Length - 1);
            }
            return query;
        }
    }
}