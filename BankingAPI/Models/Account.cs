using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAPI.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        Random rand = new Random();
        public Account()
        {
            //AccountNumberGenerated = "234" + Convert.ToString((long) Math.Floor(rand.NextDouble() * 7_000_000_000L + 1_000_000_000L));
            AccountNumberGenerated = "234" + rand.Next(1_000_000, 10_000_000).ToString();
            //AccountName = FirstName + LastName;
            AccountName = $"{FirstName} {LastName}";
        }

    }

    public enum AccountType
    {
        Savings,
        Currrent,
        Corporate,
        Government
    }
}
