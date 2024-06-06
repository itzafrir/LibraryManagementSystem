using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CD> CDs { get; set; }
        public DbSet<DVD> DVDs { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<CopiesByLocation> CopiesByLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CopiesByLocation>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CopiesByLocation>()
                .HasOne(c => c.Item)
                .WithMany(i => i.CopiesByLocations)
                .HasForeignKey(c => c.ItemId);

            base.OnModelCreating(modelBuilder);
        }
    }
}