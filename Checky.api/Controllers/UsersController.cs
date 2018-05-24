using System;
using System.Threading.Tasks;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Checky.api.View;
using Checky.api.Service.EmailService;
using Checky.api.Service.PinService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checky.api.ViewBuilder.User;

namespace Checky.api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly UserManager<Identity> identityManager;
        private readonly IUuidService pinService;
        private readonly IEmailService emailService;
        private readonly IUserViewBuilder userViewBuilder;

        public UsersController(IUserViewBuilder userViewBuilder, CheckyContext db, IUuidService pinService, IEmailService emailService, UserManager<Identity> identityManager) : base(db, identityManager)
        {
            this.pinService = pinService;
            this.emailService = emailService;
            this.identityManager = identityManager;
            this.userViewBuilder = userViewBuilder;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] View.User request) 
        {
            if (request == null) 
                throw new BadRequestException("Request Body", "Invalid request body.");

            if(string.IsNullOrEmpty(request.Email))
                throw new BadRequestException("Request Body", "Invalid email.");  
            
            if(string.IsNullOrEmpty(request.Password))
                throw new BadRequestException("Request Body", "Invalid password.");

            if (string.IsNullOrEmpty(request.Gender))
                throw new BadRequestException("Request Body", "Invalid gender.");

            if (string.IsNullOrEmpty(request.Organization.OrganizationGuid))
                throw new BadRequestException("Request Body", "Invalid organizationGuid.");

            if (string.IsNullOrEmpty(request.Birthdate))
                throw new BadRequestException("Request Body", "Invalid birthdate.");

            var organization = await db.Organizations.FirstOrDefaultAsync(x => x.OrganizationGuid == request.Organization.OrganizationGuid);

            if(organization == null)
                throw new NotFoundException("User Organization");

            var identity = new Identity
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await identityManager.CreateAsync(identity, request.Password);

            if(!result.Succeeded)
                throw new BadRequestException("User", "Unable to create user.");

            try 
            {
                var user = new Model.User
                {
                    UserGuid = identity.Id,
                    Email = identity.Email,
                    Pin = await pinService.GenerateUserPin(),
                    Gender = request.Gender,
                    Birthdate = DateTimeOffset.Parse(request.Birthdate),
                    Organization = organization
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return StatusCode(200, userViewBuilder.Build(user));
            } 
            catch (Exception e) 
            {
                await identityService.DeleteAsync(identity);
                throw new BadRequestException("User", "Unable to create user.");
            }

        }
    }
}
