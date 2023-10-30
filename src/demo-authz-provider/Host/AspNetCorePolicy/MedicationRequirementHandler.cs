using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PolicyServer.Runtime.Client;

namespace Host.AspNetCorePolicy
{
    public class MedicationRequirementHandler : AuthorizationHandler<MedicationRequirement>
    {
        private readonly IPolicyServerRuntimeClient _client;

        public MedicationRequirementHandler(IPolicyServerRuntimeClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MedicationRequirement requirement)
        {
            var user = context.User; var allowed = false;

            if (await _client.HasPermissionAsync(user, "PrescribeMedication"))
            {
                if (requirement.Amount <= 10) allowed = true;
                else allowed = await _client.IsInRoleAsync(user, "doctor");

                if (allowed || requirement.MedicationName == "placebo")
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}