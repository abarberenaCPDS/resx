using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PolicyServer.Local;

namespace PolicyServer.Runtime.Client
{
    /// <summary>
    /// PolicyServer client
    /// </summary>
    public class PolicyServerRuntimeClient : IPolicyServerRuntimeClient
    {
        private readonly Policy _policy;
        //private readonly PoliciesSrc _policies;
        private readonly IEnumerable<Policy> _policies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyServerRuntimeClient"/> class.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public PolicyServerRuntimeClient(Policy policy)
        {
            _policy = policy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyServerRuntimeClient"/> class.
        /// </summary>
        /// <param name="policies">The policies.</param>
        public PolicyServerRuntimeClient(PoliciesSrc policies)
        //public PolicyServerRuntimeClient(IEnumerable<Policy> policies)
        {
            
            this._policies = policies.Policies;
        }

        /// <summary>
        /// Determines whether the user is in a role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var policy = await EvaluateAsync(user);
            return policy.Roles.Contains(role);
        }

        /// <summary>
        /// Determines whether the user has a permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var policy = await EvaluateAsync(user);
            return policy.Permissions.Contains(permission);
        }

        /// <summary>
        /// Evaluates the policy for a given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return _policy.EvaluateAsync(user);
        }
    }
}