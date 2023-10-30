using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PolicyServer.Runtime.Client;


namespace PolicyServer.Local
{
    /// <summary>
    /// Models a Policies1
    /// </summary>
    public class PoliciesSrc
    {
        /// <summary>
        /// Gets the Policies.
        /// </summary>
        /// <value>
        /// The Policies.
        /// </value>
		public IEnumerable<Policy> Policies { get; internal set; } = new List<Policy>();
    }
}

