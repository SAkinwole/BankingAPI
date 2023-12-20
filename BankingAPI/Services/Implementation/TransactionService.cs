using BankingAPI.DataContext;
using BankingAPI.Models;
using BankingAPI.Services.Interfaces;
using BankingAPI.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BankingAPI.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(AppDbContext context, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            response.ResponseCode = "00";
            response.ResponseMessage = "Response Completed Successfully";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _context.Transactions.Where(x => x.TransactionDate == date).ToList();

            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Found successfully";
            response.Data = transaction;

            return response;



        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);

            if (authUser == null) throw new ApplicationException("Invalid Credentials");
            {
                try
                {
                    sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                    destinationAccount = _accountService.GetByAccountNumber(AccountNumber);


                    sourceAccount.CurrentAccountBalance -= Amount;
                    destinationAccount.CurrentAccountBalance += Amount;

                    if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        transaction.TransactionStatus = TranStatus.Success;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction successful";
                        response.Data = null;

                    }
                    else
                    {
                        transaction.TransactionStatus = TranStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed!";
                        response.Data = null;
                    }

                }
                catch (Exception ex)
                {

                    _logger.LogError($"AN ERROR OCCURED.............=> {ex.Message}");
                }

                transaction.TransactionType = TranType.Deposit;
                transaction.TransactionSourceAccount = _ourBankSettlementAccount;
                transaction.TransactionDestinationAccount = AccountNumber;
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                    $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                    $"TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {transaction.TransactionStatus}";


                _context.Transactions.Add(transaction);
                _context.SaveChanges();


                return response;

            }
        }
        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {

            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);

            if (authUser == null) throw new ApplicationException("Invalid Credentials");
            {
                try
                {
                    sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                    destinationAccount = _accountService.GetByAccountNumber(ToAccount);


                    sourceAccount.CurrentAccountBalance -= Amount;
                    destinationAccount.CurrentAccountBalance += Amount;

                    if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        transaction.TransactionStatus = TranStatus.Success;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction successful";
                        response.Data = null;

                    }
                    else
                    {
                        transaction.TransactionStatus = TranStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed!";
                        response.Data = null;
                    }

                }
                catch (Exception ex)
                {

                    _logger.LogError($"AN ERROR OCCURED.............=> {ex.Message}");
                }

                transaction.TransactionType = TranType.Transfer;
                transaction.TransactionSourceAccount = FromAccount;
                transaction.TransactionDestinationAccount = ToAccount;
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                    $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                    $"TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {transaction.TransactionStatus}";


                _context.Transactions.Add(transaction);
                _context.SaveChanges();


                return response;
            }

            
        }
        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();

            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);

            if (authUser == null) throw new ApplicationException("Invalid Credentials");
            {
                try
                {
                    sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                    destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);


                    sourceAccount.CurrentAccountBalance -= Amount;
                    destinationAccount.CurrentAccountBalance += Amount;

                    if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        transaction.TransactionStatus = TranStatus.Success;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction successful";
                        response.Data = null;

                    }
                    else
                    {
                        transaction.TransactionStatus = TranStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed!";
                        response.Data = null;
                    }

                }
                catch (Exception ex)
                {

                    _logger.LogError($"AN ERROR OCCURED.............=> {ex.Message}");
                }

                transaction.TransactionType = TranType.Withdrawal;
                transaction.TransactionSourceAccount = AccountNumber;
                transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                    $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                    $"TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {transaction.TransactionStatus}";

                _context.Transactions.Add(transaction);
                _context.SaveChanges();


                return response;
            }
        }
    }
}
