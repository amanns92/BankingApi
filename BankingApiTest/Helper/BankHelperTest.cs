using System;
using BankingApi.Helper;
using BankingApi.Model.Database;
using BankingApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingApiTest.Helper
{
    public class BankHelperTest
    {
        [Test]
        public void TestRandomIbanLength()
        {
            Assert.IsTrue(BankHelper.RandomIban().Length == 20);
        }

        [Test]
        public void TestRandomBicLength()
        {
            Assert.IsTrue(BankHelper.RandomBic().Length == 11);
        }
    }
}

