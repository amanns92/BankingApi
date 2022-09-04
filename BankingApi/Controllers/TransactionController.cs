using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Helper;
using BankingApi.Model.Database;
using BankingApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        public readonly IBankRepository _dbRepository;

        public TransactionController(IBankRepository db)
        {
            this._dbRepository = db;
        }

        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> CreateTransaction(string iban, double ammount)
        {
            if(ammount > 10000)
            {
                return BadRequest("User can not deposit more then 10.000$");
            }

            var account = await _dbRepository.GetAccountsByIban(iban);

            if((account.Ammount + ammount) < 100)
            {
                return BadRequest("Balance can not get lower then 100$");
            }

            if (ammount < 0 && System.Math.Abs(ammount) > ((account.Ammount/100)*90))
            {
                return BadRequest("cannot withdraw more than 90% of their total balanc");
            }

            account.Ammount += ammount;
            account.Transactions.Add(new Transaction()
            {
                Ammount = ammount,
                dateTime = DateTime.Now
            });

            return Ok(await _dbRepository.UpdateAccount(account));
        }
    }
}

