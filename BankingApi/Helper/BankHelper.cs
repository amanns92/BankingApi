using System;
namespace BankingApi.Helper
{
    public static class BankHelper
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomBic()
        {
            return RandomString(11);
        }

        public static string RandomIban()
        {
            return RandomString(20);
        }
    }
}

