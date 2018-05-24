using System;
using System.Threading.Tasks;
using Checky.api.Model;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Checky.api.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly string stripeSecretKey;

        public PaymentService(IConfiguration config)
        {
            this.stripeSecretKey = config.GetValue<string>("StripeSecretKey");
        }

        public async Task<string> ProcessPaymentByCustomerId(double total, string customerId) { 
            var chargeService = new StripeChargeService(this.stripeSecretKey);

            var charge = new StripeChargeCreateOptions
            {
                Amount = (int)(total * 100),
                Currency = "CAD",
                CustomerId = customerId
            };

            var response = await chargeService.CreateAsync(charge);

            return response.Id; 
        }

        public async Task<string> ProcessPaymentByToken(double total, string paymentToken)
        {
            var chargeService = new StripeChargeService(this.stripeSecretKey);

            var charge = new StripeChargeCreateOptions
            {
                SourceTokenOrExistingSourceId = paymentToken,
                Amount = (int)(total * 100),
                Currency = "CAD"
            };

            var response = await chargeService.CreateAsync(charge);

            return response.Id; 
        }
    }
}
