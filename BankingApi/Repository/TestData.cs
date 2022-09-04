using System;
using BankingApi.Helper;
using BankingApi.Model.Database;

namespace BankingApi.Repository
{
    public static class TestData
    {
        public static void AddTestData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<DatabaseContext>();
            if (db == null)
            {
                return;
            }

            var testUser1 = new User
            {
                UserName = "amanns",
                FirstName = "Sebastian",
                LastName = "Amann",
                Email = "sebastia.amann92@gmail.com",
                Password = "Passw0rd!",
                CreatedDate = DateTime.Now
            };

            var account1 = new Account
            {
                IBAN = BankHelper.RandomIban(),
                BIC = BankHelper.RandomBic(),
                Ammount = 1500
            };

            db.Users.Add(testUser1);
            db.Accounts.Add(account1);

            db.SaveChanges();
        }
    }
}

