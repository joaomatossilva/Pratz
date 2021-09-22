using System;
using System.Linq;

namespace Pratz.Web.Services
{
    public class IdGenerator : IIdGenerator
    {
        private static Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int length = 6;

        public string GenerateId() =>
            new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
