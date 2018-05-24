using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Checky.api.Database;
using Checky.api.Exceptions;
using Checky.api.Model;
using Checky.api.View;
using Checky.api.Service.PaymentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Checky.api.Controllers
{
    [Route("api/orders")]
    public class OrdersController : BaseController
    {
        private readonly IPaymentService paymentService;

        public OrdersController(CheckyContext db, IPaymentService paymentService, UserManager<Identity> identityService) : base(db, identityService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] View.Order request) 
        {
            var client = await GetClient();

            if (request == null)
                throw new BadRequestException("Request", "Request cannot be null.");

            if (request.User == null)
                throw new BadRequestException("Request User", "Request user cannot be null.");

            var order = new Model.Order();
            order.OrderItems = new List<Model.OrderItem>();

            var organizationItems = await db.OrganizationItems.Where(x => x.Organization == client.Organization).Select(x => x.Item).ToListAsync();

            var device = await db.Devices
                                          .Include(x => x.Inventory)
                                            .ThenInclude(x => x.InventoryItems)
                                            .ThenInclude(x => x.Item)
                                          .FirstOrDefaultAsync(x => x.DeviceGuid == client.User.UserGuid);

            var inventoryItems = device.Inventory.InventoryItems.ToList();
            var orderItems = request.OrderItems.ToList();

            foreach(var orderItem in orderItems)
            {
                var inventoryItem = inventoryItems.FirstOrDefault(x => x.Item.ItemGuid == orderItem.Item.ItemGuid);

                if (inventoryItem == null)
                    throw new BadRequestException("OrderItem", "One or more order items are not part of the device inventory.");

                if (inventoryItem.Quantity < int.Parse(orderItem.Quantity))
                    throw new BadRequestException("Quantity", "Inventory does not contain required amount of items to complete order.");

                order.OrderItems.Add(new Model.OrderItem {
                    Item = inventoryItem.Item,
                    Quantity = int.Parse(orderItem.Quantity)
                });

                inventoryItem.Quantity -= int.Parse(orderItem.Quantity);
                db.InventoryItems.Update(inventoryItem);
            }

            db.Orders.Add(order);

            Model.PaymentMethod paymentMethod;

            string transactionToken = "";

            if (request.User.Pin != null)
            {
                var user = await db.Users
                                   .Include(x => x.PaymentMethods)
                                   .FirstOrDefaultAsync(x => x.Pin == request.User.Pin);

                if(user == null)
                    throw new NotFoundException("User");

                paymentMethod = user.PaymentMethods.FirstOrDefault(x => x.Default == true);

                if (paymentMethod == null)
                    throw new NotFoundException("Default PaymentMethod");

                transactionToken = await paymentService.ProcessPaymentByCustomerId(0, paymentMethod.Details);
            } else {
                if (request.User.PaymentMethods == null || !request.User.PaymentMethods.Any())
                    throw new BadRequestException("PaymentMethod", "Payment method expected.");

                var paymentMethodFromRequest = request.User.PaymentMethods.First();

                var anonymousUser = await db.Users.FirstOrDefaultAsync(x => x.Email == "ANONYMOUS_USER");

                if (anonymousUser == null)
                {
                    anonymousUser = new Model.User
                    {
                        Email = "ANONYMOUS_USER",
                        Organization = client.Organization
                    };
                }

                paymentMethod = new Model.PaymentMethod
                {
                    Type = paymentMethodFromRequest.Type,
                    User = anonymousUser
                };

                db.PaymentMethods.Add(paymentMethod);

                transactionToken = await paymentService.ProcessPaymentByToken(0, paymentMethod.Details);
            }

            if (transactionToken == "")
                throw new BadRequestException("Payment", "Unable to process payment");

            var transaction = new Transaction
            {
                Total = 0,
                TransactionToken = transactionToken
            };

            await db.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
