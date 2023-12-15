using BankingAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.DTOs
{
    public class UpdateAccountDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateLastUpdated { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]/d{4}$", ErrorMessage = "Pin must not be more than 4 digits")]
        public string Pin { get; set; }

        [Required]
        [Compare("Pin", ErrorMessage = "Pin does not match")]
        public string ConfirmPin { get; set; }
    }
}
