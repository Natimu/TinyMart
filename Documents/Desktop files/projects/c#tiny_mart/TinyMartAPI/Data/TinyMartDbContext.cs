using Microsoft.EntityFrameworkCore;
using TinyMartAPI.Models;

namespace TinyMartAPI.Data
{
    public class TinyMartDbContext : DbContext
    {
        public TinyMartDbContext(DbContextOptions<TinyMartDbContext> options) : base(options) { }

        // Tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<AudioProduct> AudioProducts { get; set; }
        public DbSet<VideoProduct> VideoProducts { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<PaperBook> PaperBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<AudioProduct>("Audio")
                .HasValue<VideoProduct>("Video")
                .HasValue<EBook>("EBook")
                .HasValue<PaperBook>("PaperBook");

            modelBuilder.Owned<NameType>();

            modelBuilder.Entity<Cart>().OwnsOne(c => c.Owner);

            modelBuilder.Entity<Cart>()
                 .HasMany(c => c.Items)
                 .WithOne()
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
