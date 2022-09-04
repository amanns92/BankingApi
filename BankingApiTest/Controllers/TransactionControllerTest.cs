using System;
using BankingApi.Controllers;
using BankingApi.Helper;
using BankingApi.Model.Database;
using BankingApi.Repository;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApiTest.Controllers
{
    public class TransactionControllerTest
    {
        private static readonly string ACCOUNT_ONE_IBAN = BankHelper.RandomIban();

        private BankingRepository bankingRepository;
        private TransactionController controller;

        [SetUp]
        public void Setup()
        {
            var option = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;

            var context = new DatabaseContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
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
                IBAN = ACCOUNT_ONE_IBAN,
                BIC = BankHelper.RandomBic(),
                Ammount = 1500
            };

            context.Users.Add(testUser1);
            context.Accounts.Add(account1);

            context.SaveChanges();

            bankingRepository = new BankingRepository(context);

            controller = new TransactionController(bankingRepository);
        }

        [Test]
        public void depositTest()
        {
            var response = controller.CreateTransaction(ACCOUNT_ONE_IBAN, 100).Result;

            var account = bankingRepository.GetAccountsByIban(ACCOUNT_ONE_IBAN).Result;

            Assert.IsTrue(account.Transactions.Count == 1);
            Assert.AreEqual(account.Transactions.First().Ammount, 100);
            Assert.AreEqual(account.Ammount, 1600);
        }

        [Test]
        public void depositToBigTest()
        {
            var response = controller.CreateTransaction(ACCOUNT_ONE_IBAN, 10001).Result;

            var account = bankingRepository.GetAccountsByIban(ACCOUNT_ONE_IBAN).Result;

            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.IsTrue(account.Transactions.Count == 0);
            Assert.AreEqual(account.Ammount, 1500);
        }

        [Test]
        public void withdrawMoreThen90PercentTest()
        {
            var response = controller.CreateTransaction(ACCOUNT_ONE_IBAN, -1351).Result;

            var account = bankingRepository.GetAccountsByIban(ACCOUNT_ONE_IBAN).Result;

            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.IsTrue(account.Transactions.Count == 0);
            Assert.AreEqual(account.Ammount, 1500);
        }
    }
}
