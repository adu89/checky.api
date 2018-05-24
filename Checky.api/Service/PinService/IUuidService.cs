using System;
using System.Threading.Tasks;

namespace Checky.api.Service.PinService
{
    public interface IUuidService
    {
        Task<string> GenerateUserPin();
        Task<string> GenerateMachineId();
    }
}
