using Microsoft.EntityFrameworkCore;
using TravelEasy.Models;

namespace TravelEasy.Data
{
    public class TravelEasyContext : DbContext
    {
        public TravelEasyContext(DbContextOptions<TravelEasyContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Shelving> Shelvings { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Benefit> Benefits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Relazione tra Comment e User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relazione tra Comment e BlogPost
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relazione tra Area e Shelving
            modelBuilder.Entity<Area>()
                .HasMany(a => a.Shelvings)
                .WithOne(s => s.Area)
                .HasForeignKey(s => s.AreaId)
                .OnDelete(DeleteBehavior.Cascade); // Cascata di cancellazione per Shelving

            // Relazione tra Shelving e Shelf
            modelBuilder.Entity<Shelving>()
                .HasMany(s => s.Shelves)
                .WithOne(s => s.Shelving)
                .HasForeignKey(s => s.ShelvingId)
                .OnDelete(DeleteBehavior.Cascade); // Cascata di cancellazione per Shelf

            // Relazione tra Product e Area
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Area)
                .WithMany(a => a.Products)
                .HasForeignKey(p => p.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relazione tra Product e Shelving
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shelving)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShelvingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relazione tra Product e Shelf
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shelf)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShelfId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relazione tra Area e Shelving
            modelBuilder.Entity<Area>()
                .HasMany(a => a.Shelvings)
                .WithOne(s => s.Area)
                .HasForeignKey(s => s.AreaId)
                .OnDelete(DeleteBehavior.Cascade); // Cascata di cancellazione per Shelving

            // Relazione tra Shelving e Shelf
            modelBuilder.Entity<Shelving>()
                .HasMany(s => s.Shelves)
                .WithOne(s => s.Shelving)
                .HasForeignKey(s => s.ShelvingId)
                .OnDelete(DeleteBehavior.Cascade); // Cascata di cancellazione per Shelf

            // Relazione tra Review e Image
            modelBuilder.Entity<Review>()
                .HasMany(r => r.ReviewImages)
                .WithOne(i => i.Review)
                .HasForeignKey(i => i.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);  // Usa Restrict per prevenire cicli

            // Relazione tra Review e Video
            modelBuilder.Entity<Review>()
                .HasMany(r => r.ReviewVideos)
                .WithOne(v => v.Review)
                .HasForeignKey(v => v.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);  // Usa Restrict per prevenire cicli


            // Relazione tra Image e BlogPost
            modelBuilder.Entity<Image>()
                .HasOne(i => i.BlogPost)
                .WithMany(b => b.Images)
                .HasForeignKey(i => i.BlogPostId)
                .OnDelete(DeleteBehavior.SetNull);  // SetNull per evitare cancellazioni a cascata
        }


    }
}


