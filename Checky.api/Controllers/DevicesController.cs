using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Checky.api.Service;
using Checky.api.View;
using Checky.api.ViewBuilder.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Checky.api.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : BaseController
    {
        private readonly IPasswordService passwordService;
        private readonly UserManager<Identity> identityManager;
        private readonly IDeviceViewBuilder deviceViewBuilder;

        public DevicesController(IDeviceViewBuilder deviceViewBuilder, UserManager<Identity> identityManager, IPasswordService passwordService, CheckyContext db) : base(db, identityManager) 
        {
            this.identityManager = identityManager;
            this.passwordService = passwordService;
            this.deviceViewBuilder = deviceViewBuilder;
        }

        /* This method will be used to sync a device.
         * Syncing a device comprises of serveral steps
         *  1. Creating the device credentials (Ability to do this is based on organization license.
         *     A new device can only be registered if the organizations license allows them to do so.
         *     An organization's license has a max device count.
         *  2. Creating the device inventory based on the organization's inventory.
         *     The inventory will be initialised with each item set to quantity 0.
         * 
         * Only an AppAdmin or OrgAdmin can sync a device. AppAdmins must pass the devices organization to set up a
         * device properly.
         */
        [HttpPost] 
        [Authorize]
        public async Task<IActionResult> Sync([FromBody] View.Device request) 
        {
            var client = await GetClient();
            var organization = client.Organization;

            if (client.Role != "AppAdmin" && client.Role != "OrgAdmin")
                throw new BadRequestException("Client", "Client does not have permission to access this resource.");

            if(client.Role == "AppAdmin") {
                if(request == null || string.IsNullOrEmpty(request.Organization.OrganizationGuid))
                    throw new BadRequestException("Organization", "Please provice an organization to continue.");

                var appAdminOrganization = await db.Organizations.FirstOrDefaultAsync(x => x.OrganizationGuid == request.Organization.OrganizationGuid);

                if(appAdminOrganization == null) 
                    throw new BadRequestException("Organization", "Organization does not exist.");

                organization = appAdminOrganization;
            }

            if (string.IsNullOrEmpty(request.DeviceName))
                throw new BadRequestException("deviceName", "Device name required to complete request");

            var syncdDevicesCount = await db.Devices.Where(x => x.Organization == organization).CountAsync();
            var maxDevicesCount = organization.MaxDevices;

            if (syncdDevicesCount >= maxDevicesCount)
                throw new BadRequestException("Organization", "Maximum devices synced according to license.");

            var deviceNameExists = await db.Devices
                                           .AnyAsync(x => x.DeviceName == request.DeviceName && x.Organization == organization);

            if (deviceNameExists)
                throw new BadRequestException("deviceName", "Device already exists with that name");

            var deviceGuid = Guid.NewGuid();
            var password = passwordService.GenerateSecurePassword();

            var identity = new Identity
            {
                UserName = deviceGuid.ToString(),
                Email = string.Format("{0}@{0}.com", deviceGuid.ToString())
            };

            var result = await identityManager.CreateAsync(identity, password);

            if (!result.Succeeded)
                throw new BadRequestException("Device", "Unable to sync device");

            try
            {
                List<Model.InventoryItem> inventoryItems = await db.OrganizationItems
                                                             .Where(x => x.Organization == organization)
                                                             .Select(x => new Model.InventoryItem
                                                             {
                                                                 Item = x.Item,
                                                                 Quantity = 0
                                                             }).ToListAsync();

                var inventory = new Model.Inventory
                {
                    Organization = organization,
                    InventoryItems = inventoryItems
                };

                var device = new Model.Device
                {
                    DeviceGuid = deviceGuid.ToString(),
                    Organization = organization,
                    Inventory = inventory,
                    DeviceName = request.DeviceName
                };

                db.Devices.Add(device);

                await db.SaveChangesAsync();

                var deviceView = deviceViewBuilder.Build(device);
                deviceView.Password = password;
                return StatusCode(200, deviceView);

            } catch
            {
                await identityManager.DeleteAsync(identity);
                throw new BadRequestException("Device", "Unable to sync device");
            }
        }

        [HttpGet("{deviceGuid}")]
        public async Task<IActionResult> Get([FromRoute] string deviceGuid)
        {
            var client = await GetClient();

            if(client.Role != "Device")
                throw new BadRequestException("Client", "Client does not have permission to access this resource.");

            var device = await db.Devices
                                 .Include(x => x.Inventory)
                                 .Include(x => x.Inventory.InventoryItems)
                                    .ThenInclude(x => x.Item)
                                    .ThenInclude(x => x.Vendor)
                                 .FirstOrDefaultAsync(x => x.DeviceGuid == deviceGuid);

            if(device == null)
                throw new NotFoundException("Device");

            return StatusCode(200, deviceViewBuilder.Build(device));
        }
    }
}
