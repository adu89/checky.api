using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Checky.api.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checky.api.Controllers
{
    [Route("api/inventory-items")]
    public class InventoryItemsController: BaseController
    {
        public InventoryItemsController(CheckyContext db, UserManager<Identity> userManager) : base(db, userManager) {
        }

        [HttpPut("{inventoryItemGuid}")]
        public async Task<IActionResult> Update([FromRoute] string inventoryItemGuid, [FromBody] View.InventoryItem request) {            
            var client = await GetClient();

            if (request == null)
                throw new BadRequestException("Request", "Request cannot be null.");

            if (client.Role != "AppAdmin" && client.Role != "OrgAdmin")
                throw new NotAuthorizedException("User");

            var inventoryItem = await db.InventoryItems
                                        .FirstOrDefaultAsync(x => x.InventoryItemGuid == inventoryItemGuid);

            if (inventoryItem == null)
                throw new NotFoundException("InventoryItem");

            var quantity = 0;

            if (!int.TryParse(request.Quantity, out quantity))
                throw new BadRequestException("Quantity", "Invalid quantity provided.");

            inventoryItem.Quantity = quantity;

            db.InventoryItems.Update(inventoryItem);

            await db.SaveChangesAsync();

            return StatusCode(200, inventoryItem);
        }
    }
}
