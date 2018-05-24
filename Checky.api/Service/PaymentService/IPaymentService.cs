using System;
using System.Threading.Tasks;
using Checky.api.Model;

namespace Checky.api.Service.PaymentService
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentByToken(double d, string s);
        Task<string> ProcessPaymentByCustomerId(double d, string s);

    }
}
