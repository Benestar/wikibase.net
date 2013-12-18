using System;
using System.Collections.Generic;
using System.Text;

namespace Wikibase
{
    /// <summary>
    /// Specific exception raised by the the wikibase API.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="ApiException"/>.
        /// </summary>
        public ApiException() : base()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ApiException"/> with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ApiException(String message)
            : base(message)
        {
        }
    }
}
