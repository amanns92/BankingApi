using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Model;
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
    public class UserController : ControllerBase
    {
        public readonly IBankRepository _dbRepository;

        public UserController(IBankRepository db)
        {
            this._dbRepository = db;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> AddUser(User user)
        {
            var u = await _dbRepository.AddUser(user);

            if (u == null)
            {
                return BadRequest();
            }

            return Ok(u);
        }

        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var u = await _dbRepository.UpdateUser(user);

            if (u == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(u);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateUser user)
        {
            var token = await _dbRepository.Authenticate(user);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

    }
}

