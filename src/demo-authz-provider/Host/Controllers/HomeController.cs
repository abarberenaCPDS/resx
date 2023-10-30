using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Host.AspNetCorePolicy;
using PolicyServer.Runtime.Client;

namespace Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPolicyServerRuntimeClient _client;
        private readonly IAuthorizationService _authz;

        public HomeController(IPolicyServerRuntimeClient client, IAuthorizationService authz)
        {
            _client = client;
            _authz = authz;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Secure()
        {
            var result = await _client.EvaluateAsync(User);
            return View(result);
        }

        // if you are using the UsePolicyServerClaims middleware, roles are mapped to claims
        // this allows using the classic authorize attribute
        [Authorize(Roles = "Dval Staff")]
        public async Task<IActionResult> Staff()
        {
            // can also use the client library imperatively
            var isStaff = await _client.IsInRoleAsync(User, "Dval Staff");

            return View("success");
        }

        // the preferred approach is to use the authorization policy system in ASP.NET Core
        // if you add the AuthorizationPermissionPolicies service, policy names are automatically mapped to permissions
        [Authorize("CanDeleteOrders")]
        public async Task<IActionResult> DeleteOrders()
        {
            // or imperatively
            var canDeleteOrders = await _client.HasPermissionAsync(User, "CanDeleteOrders");

            return View("success");
        }

        [Authorize("CanDeleteMyOwnOrders")]
        public async Task<IActionResult> CanDeleteMyOwnOrders()
        {
            // or imperatively
            var canDeleteMyOwnOrders = await _client.HasPermissionAsync(User, "CanDeleteMyOwnOrders");

            return View("success");
        }

        [Authorize("CanExportOrderToPDF")]
        public async Task<IActionResult> CanExportOrderToPDF()
        {
            // or imperatively
            var canDeleteMyOwnOrders = await _client.HasPermissionAsync(User, "CanExportOrderToPDF");

            return View("success");
        }

        [Authorize("CanViewMyOwnOrders")]
        public async Task<IActionResult> CanViewMyOwnOrders()
        {
            // or imperatively
            var canDeleteMyOwnOrders = await _client.HasPermissionAsync(User, "CanViewMyOwnOrders");

            return View("success");
        }

        public async Task<IActionResult> PrescribeMedication(string name, int amount)
        {
            var requirement = new MedicationRequirement
            {
                Amount = amount,
                MedicationName = name
            };
            var result = await _authz.AuthorizeAsync(User, null, requirement);
            if (!result.Succeeded) return Forbid();

            return View("success");
        }
    }
}