using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Model.Database
{
    public class Account
    {
        [Key]
        [StringLength(255)]
        public string IBAN { get; set; } = "";
        [StringLength(255)]
        [Required]
        public string BIC { get; set; } = "";
        [Required]
        public Double Ammount { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

