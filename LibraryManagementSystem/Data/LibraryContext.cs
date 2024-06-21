using System;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }

        // Parameterless constructor needed for design-time tools
        public LibraryContext()
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"C:\Users\ItayTzafrir\source\repos\LibraryManagementSystem\LibraryManagementSystem\library.db")
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
    }
}