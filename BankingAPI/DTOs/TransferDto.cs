namespace BankingAPI.DTOs
{
    public class TransferDto
    {
        public string FromAccount;
        public string ToAccount;
        public decimal Amount;
        public string TransactionPin;
    }
}
