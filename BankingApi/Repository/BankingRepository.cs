using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BankingApi.Helper;
using BankingApi.Model;
using BankingApi.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace BankingApi.Repository
{
    public class BankingRepository : IBankRepository
    {
        private readonly DatabaseContext _dbContext;

        public BankingRepository(DatabaseContext databaseContext)
        {
            this._dbContext = databaseContext;
        }

        public async Task<Account?> AddAccount(string userName, double ammount)
        {
            var acc = new Account()
            {
                Ammount = ammount,
                BIC = BankHelper.RandomBic(),
                IBAN = BankHelper.RandomIban()
            };
            var user = await GetUserByUsername(userName);
            if(user == null)
            {
                return null;
            }
            user.Accounts.Add(acc);

            _dbContext.Set<User>().Update(user);
            await _dbContext.SaveChangesAsync();

            return acc;
        }

        public async Task<Account?> UpdateAccount(Account account)
        {
            _dbContext.Set<Account>().Update(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        public async Task<Transaction?> AddTransaction(string iban, Transaction transaction)
        {
            var acc = await GetAccountsByIban(iban);
            if (acc == null)
            {
                return null;
            }
            acc.Transactions.Add(transaction);

            _dbContext.Set<Account>().Update(acc);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public async Task<User?> AddUser(User blog)
        {
            _dbContext.Set<User>().Add(blog);
            await _dbContext.SaveChangesAsync();
            return blog;
        }

        public async Task<Token?> Authenticate(AuthenticateUser user)
        {
            if (!_dbContext.Set<User>().Any(x => x.UserName == user.Name && x.Password == user.Password))
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["JWTKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
             new Claim(ClaimTypes.Name, user.Name)
              }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { JWT = tokenHandler.WriteToken(token) };
        }

        public async Task<Account?> DeleteAccountByIban(string iban)
        {
            var entity = await _dbContext.Set<Account>().FindAsync(iban);
            if (entity == null)
            {
                return entity;
            }

            _dbContext.Set<Account>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Transaction?> DeleteTransactionById(string id)
        {
            var entity = await _dbContext.Set<Transaction>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dbContext.Set<Transaction>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<User?> DeleteUserByUsername(string userName)
        {
            var entity = await _dbContext.Set<User>().FindAsync(userName);
            if (entity == null)
            {
                return entity;
            }

            _dbContext.Set<User>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Account?> GetAccountsByIban(string iban)
        {
            return await _dbContext.Set<Account>().FindAsync(iban);
        }

        public async Task<List<Account>> GetAccountsByUsername(string userName)
        {
            return _dbContext.Set<User>().FindAsync(userName).Result?.Accounts.ToList() ?? new List<Account>();
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _dbContext.Set<Transaction>().ToListAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public async Task<Transaction?> GetTransactionById(string id)
        {
            return await _dbContext.Set<Transaction>().FindAsync(id);
        }

        public async Task<List<Transaction>> GetTransactionsByIban(string iban)
        {
            var account = await _dbContext.Set<Account>().FindAsync(iban);
            return account?.Transactions.ToList() ?? new List<Transaction>();
        }

        public async Task<User?> GetUserByUsername(string userName) => await _dbContext.Set<User>().FindAsync(userName);

        public async Task<User?> UpdateUser(User user)
        {
            _dbContext.Set<User>().Update(user);
            return user;
        }
    }
}

