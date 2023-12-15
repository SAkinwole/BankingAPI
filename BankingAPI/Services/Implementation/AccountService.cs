using BankingAPI.DataContext;
using BankingAPI.Models;
using BankingAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BankingAPI.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        public AccountService(AppDbContext context)
        {
            _context = context;
        }
        public Account Authenticate(string AccountNumber, string Pin)
        {
            var account = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (account == null)
                return null;

            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;

            return account;

        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentException("Pin");
            using (var hmac = new HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            if (_context.Accounts.Any(x => x.Email == account.Email))
                throw new ArgumentException("An account already exists with this email");

            if (!Pin.Equals(ConfirmPin))
                throw new ArgumentException("Pins do not match", "Pin");

            Byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);


            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }



        public void Delete(int Id)
        {
            var account = _context.Accounts.Find(Id);
            if (account != null)
                _context.Accounts.Remove(account);
                _context.SaveChanges();
        }

        public IEnumerable<Account> GetAllAccount()
        {
            return _context.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public Account GetById(int Id)
        {
            var account = _context.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var accountToUpdate = _context.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToUpdate == null)
                throw new ApplicationException("Account does not exist");

            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_context.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("This Email" + account.Email + "already exists");

                accountToUpdate.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_context.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("This Phone Number" + account.PhoneNumber + "already exists");

                accountToUpdate.PhoneNumber = account.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;

                CreatePinHash(Pin, out pinHash, out pinSalt);


                accountToUpdate.PinHash = account.PinHash;
                accountToUpdate.PinSalt = account.PinSalt;
            }
            accountToUpdate.DateLastUpdated = DateTime.Now();

            _context.Accounts.Update(accountToUpdate);
            _context.SaveChanges();
        }
    }
}
