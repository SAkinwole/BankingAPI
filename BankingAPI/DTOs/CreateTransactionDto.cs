using BankingAPI.Models;

namespace BankingAPI.DTOs
{
    public class CreateTransactionDto
    {
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
