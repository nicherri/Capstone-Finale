using System.ComponentModel.DataAnnotations;

namespace TravelEasy.Models.DTO
{
    public class UserRegistrationDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }
    }

}
