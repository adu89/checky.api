using System;
namespace Checky.api.Service
{
    public interface IPasswordService
    {
        string GenerateSecurePassword();
    }
}
