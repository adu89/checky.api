using System;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checky.api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly CheckyContext db;
        protected readonly UserManager<Identity> identityService;

        public BaseController(CheckyContext db, UserManager<Identity> identityService)
        {
            this.db = db;
            this.identityService = identityService;
        }

        protected async Task<Client> GetClient()
        {

            var tokenUserGuid = User.GetClaim(OpenIdConnectConstants.Claims.Subject);
            var tokenRole = User.GetClaim(OpenIdConnectConstants.Claims.Role);
            
            var identity = await identityService.FindByIdAsync(tokenUserGuid);
            var user = await db.Users
                               .Include(x => x.Organization)
                               .FirstOrDefaultAsync(x => x.UserGuid == tokenUserGuid);

            if (identity != null && user != null)
            {
                return new Client
                {
                    Identity = identity,
                    User = user,
                    Role = tokenRole
                };
            }

            throw new BadRequestException("Token", "Invalid authentication token.");
        }
    }
}
