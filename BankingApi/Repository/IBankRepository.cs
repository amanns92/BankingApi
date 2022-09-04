using System;
using System.Threading.Tasks;
using BankingApi.Model;
using BankingApi.Model.Database;

namespace BankingApi.Repository
{
    public interface IBankRepository
    {
        Task<Token?> Authenticate(AuthenticateUser users);


        Task<User?> GetUserByUsername(string userName);

        Task<User?> DeleteUserByUsername(string iduserName);

        Task<List<User>> GetAllUsers();

        Task<User?> AddUser(User user);

        Task<User?> UpdateUser(User user);


        Task<List<Account>> GetAccountsByUsername(string userName);

        Task<Account?> GetAccountsByIban(string iban);

        Task<Account?> DeleteAccountByIban(string iban);

        Task<Account?> AddAccount(string userName, double ammount);

        Task<Account?> UpdateAccount(Account account);


        Task<Transaction?> GetTransactionById(string id);

        Task<List<Transaction>> GetTransactionsByIban(string Iban);

        Task<Transaction?> DeleteTransactionById(string id);

        Task<List<Transaction>> GetAllTransactions();

        Task<Transaction?> AddTransaction(string iban, Transaction transaction);
    }
}

