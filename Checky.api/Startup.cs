using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Checky.api.Database;
using Checky.api.Middleware;
using Checky.api.Service.PinService;
using Checky.api.Service.EmailService;
using Checky.api.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Checky.api.ViewBuilder.User;
using Checky.api.ViewBuilder.Device;
using Checky.api.Service.PasswordService;
using Checky.api.Service;
using Checky.api.Service.StorageService;

namespace Checky.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CheckyContext>(options =>
            {
                options.UseSqlServer(connection);
                options.UseOpenIddict();
            });

            services.AddIdentity<Identity, IdentityRole>()
                .AddEntityFrameworkStores<CheckyContext>()
                .AddDefaultTokenProviders();

            // Register the OpenIddict services.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            services.AddOpenIddict().AddCore(options =>
            {
                options.UseDefaultModels();
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<CheckyContext>();
            }).AddServer(options =>
            {
                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint (required to use the password flow).
                options.EnableTokenEndpoint("/api/auth");

                // Allow client applications to use the grant_type=password flow.
                options.AllowPasswordFlow();

                options.AllowRefreshTokenFlow();

                options.SetAccessTokenLifetime(new TimeSpan(365, 0, 0));

                options.DisableHttpsRequirement();
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = OAuthValidationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OAuthValidationDefaults.AuthenticationScheme;
            })
            .AddOAuthValidation();

            services.Configure<IdentityOptions>(
                options =>
                {
                    options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                    options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    //options.Password.RequireNonAlphanumeric = false;
                    //options.Password.RequireUppercase = true;
                    //options.Password.RequireLowercase = false;

                }
            );

            //Configuration found in appsettings.json
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IUuidService, UuidService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IDeviceViewBuilder, DeviceViewBuilder>();
            services.AddScoped<IInventoryViewBuilder, InventoryViewBuilder>();
            services.AddScoped<IInventoryItemViewBuilder, InventoryItemViewBuilder>();
            services.AddScoped<IItemViewBuilder, ItemViewBuilder>();
            services.AddScoped<IOrganizationViewBuilder, OrganizationViewBuilder>();
            services.AddScoped<IUserViewBuilder, UserViewBuilder>();
            services.AddScoped<IVendorViewBuilder, VendorViewBuilder>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
