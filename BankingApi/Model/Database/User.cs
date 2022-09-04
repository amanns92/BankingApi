using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankingApi.Model.Database
{
    public class User
    {
        [Key]
        [StringLength(25)]
        public string UserName { get; set; } = "";
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; } = "";
        [Required]
        [StringLength(25)]
        public string LastName { get; set; } = "";
        [StringLength(255)]
        public string Email { get; set; } = "";
        [JsonIgnore]
        [Required]
        public string Password { get; set; } = "";
        public DateTime CreatedDate { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}

