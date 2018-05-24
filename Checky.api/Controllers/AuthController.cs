using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Checky.api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly CheckyContext db;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly SignInManager<Identity> signInManager;
        private readonly UserManager<Identity> userManager;

        public AuthController(
        IOptions<IdentityOptions> identityOptions, SignInManager<Identity> signInManager, UserManager<Identity> userManager, CheckyContext db) : base()
        {
            this.identityOptions = identityOptions;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    throw new BadRequestException("User", "Password or email error.");
                }

                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    throw new BadRequestException("User", "Password or email error.");
                }

                var checkyUser = await db.Users
                                   .Include(x => x.UserRoles)
                                   .ThenInclude(x => x.Role)
                                   .Include(x => x.Organization)
                                   .FirstOrDefaultAsync(x => x.UserGuid == user.Id);

                // Create a new ClaimsIdentity holding the user identity.
                var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme, OpenIdConnectConstants.Claims.Name, OpenIdConnectConstants.Claims.Role);
                // Add a "sub" claim containing the user identifier, and attach
                // the "access_token" destination to allow OpenIddict to store it
                // in the access token, so it can be retrieved from your controllers.
                identity.AddClaim(OpenIdConnectConstants.Claims.Subject, user.Id, OpenIdConnectConstants.Destinations.AccessToken);

                var role = "User";

                if (checkyUser.UserRoles.Any(x => x.Role.RoleName == "AppAdmin"))
                {
                    role = "AppAdmin";
                } 
                else if (checkyUser.UserRoles.Any(x => x.Role.RoleName == "OrgAdmin"))
                {
                    role = "OrgAdmin";
                }
                else if (checkyUser.UserRoles.Any(x => x.Role.RoleName == "Device"))
                {
                    role = "Device";
                }

                identity.AddClaim(OpenIdConnectConstants.Claims.Role, role, OpenIdConnectConstants.Destinations.AccessToken);

                var principal = new ClaimsPrincipal(identity);
                // Ask OpenIddict to generate a new token and return an OAuth2 token response.
                try
                {
                    return SignIn(principal, OpenIdConnectServerDefaults.AuthenticationScheme);
                }
                catch (Exception e)
                {
                    throw new BadRequestException("User", "Unable to sign in.");

                }
            }

            throw new BadRequestException("User", "Unable to process authorization request.");
        }
    }
}
