using System.ComponentModel.DataAnnotations;

namespace TravelEasy.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Cognome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public ICollection<Wishlist> Wishlists { get; set; }
    }

    public enum UserRole
    {
        Customer,
        Admin,
        ProductManager,
        OrderManager,
        Writer
    }

}
