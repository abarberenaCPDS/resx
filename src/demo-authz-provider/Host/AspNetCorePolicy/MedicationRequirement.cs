using Microsoft.AspNetCore.Authorization;

namespace Host.AspNetCorePolicy
{
    public class MedicationRequirement : IAuthorizationRequirement
    {
        public string MedicationName { get; set; }
        public int Amount { get; set; }
    }
}