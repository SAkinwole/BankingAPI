using System.ComponentModel.DataAnnotations;

namespace BankingAPI.DTOs
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{0}$")]
        public string AccountNumber { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
