using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PolicyServer.Runtime.Client;

namespace PolicyServer.Local
{
    /// <summary>
    /// Models a policy
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// The Id of the Policy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name of the Policy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; internal set; } = new List<Role>();

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permission> Permissions { get; internal set; } = new List<Permission>();
        
        internal Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = Roles.Where(x=> x.Evaluate(user)).Select(x => x.Name).ToArray();
            var permissions = Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return Task.FromResult(result);
        }
    }
}