using System;
using System.Linq;
using System.Threading.Tasks;
using Checky.api.Database;
using Microsoft.EntityFrameworkCore;

namespace Checky.api.Service.PinService
{
    public class UuidService : IUuidService
    {
        private readonly CheckyContext db;
        private readonly string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

        public UuidService(CheckyContext db) 
        {
            this.db = db;   
        }

        public async Task<string> GenerateMachineId()
        {
            var pin = "";
            do
            {
                var tempPin = getRandomString(15);

                var pinExists = await db.Users.AnyAsync(x => x.Pin == tempPin);

                if (!pinExists) pin = tempPin;

            } while (pin != "");

            return pin;
        }

        public async Task<string> GenerateUserPin()
        {
            var pin = "";
            do
            {
                var tempPin = getRandomString(5);

                var pinExists = await db.Users.AnyAsync(x => x.Pin == tempPin);

                if (!pinExists) pin = tempPin;

            } while (pin == "");

            return pin;
        }

        private string getRandomString(int length) 
        {
            char[] chars = new char[length];

            Random rd = new Random();

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars); 
        }
    }
}
