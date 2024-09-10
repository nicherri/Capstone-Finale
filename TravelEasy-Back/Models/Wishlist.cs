using System.ComponentModel.DataAnnotations;

namespace TravelEasy.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
}