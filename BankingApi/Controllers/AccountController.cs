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
    public class AccountController : ControllerBase
    {
        public readonly IBankRepository _dbRepository;

        public AccountController(IBankRepository db)
        {
            this._dbRepository = db;
        }

        [HttpPost]
        [Route("addaccount")]
        public async Task<IActionResult> AddAccount(double ammount)
        {
            var acc = new Account()
            {
                Ammount = ammount,
                IBAN = BankHelper.RandomIban(),
                BIC = BankHelper.RandomBic()
            };

            var user = await _dbRepository.GetUserByUsername(User?.Identity?.Name ?? "");
            if (user == null)
            {
                return BadRequest("Could not find User");
            }

            user.Accounts.Add(acc);

            return Ok(await _dbRepository.UpdateUser(user));
        }

        [HttpPost]
        [Route("deleteaccount")]
        public async Task<IActionResult> DeleteAccount(string iban)
        {
            var user = await _dbRepository.GetUserByUsername(User?.Identity?.Name ?? "");
            if (user == null)
            {
                return BadRequest("Could not find User");
            }

            user.Accounts.Remove(await _dbRepository.GetAccountsByIban(iban));

            return Ok(await _dbRepository.UpdateUser(user));
        }
    }
}

