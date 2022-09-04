using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Model
{
    public class AuthenticateUser
    {
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
    }
}

