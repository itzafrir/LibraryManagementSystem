using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CD> CDs { get; set; }
        public DbSet<DVD> DVDs { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRequest> LoanRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<FinePayRequest> FinePayRequests { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=library.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasKey(i => i.Id);
            modelBuilder.Entity<Loan>().HasKey(l => l.Id);
            modelBuilder.Entity<LoanRequest>().HasKey(lr => lr.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Review>().HasKey(r => r.Id);
            modelBuilder.Entity<Fine>().HasKey(f => f.Id);
            modelBuilder.Entity<FinePayRequest>().HasKey(fpr => fpr.Id);

            modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Book>("Book")
                .HasValue<CD>("CD")
                .HasValue<DVD>("DVD")
                .HasValue<EBook>("EBook")
                .HasValue<Magazine>("Magazine");

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Reviews)
                .HasForeignKey(r => r.ItemId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.ItemId })
                .IsUnique(); // Add unique constraint on UserId and ItemId

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany(u => u.Fines)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FinePayRequest>()
                .HasOne(fpr => fpr.User)
                .WithMany()
                .HasForeignKey(fpr => fpr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore collections that are not entities
            modelBuilder.Entity<Book>().Ignore(b => b.Keywords);
            modelBuilder.Entity<CD>().Ignore(c => c.Tracks);
            modelBuilder.Entity<DVD>().Ignore(d => d.Subtitles);
            modelBuilder.Entity<DVD>().Ignore(d => d.Cast);
            modelBuilder.Entity<EBook>().Ignore(e => e.Keywords);
            modelBuilder.Entity<Magazine>().Ignore(m => m.Articles);
        }
    }
}
