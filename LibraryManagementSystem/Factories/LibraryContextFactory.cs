using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManagementSystem.Data
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(@"C:\Users\ItayTzafrir\source\repos\LibraryManagementSystem\LibraryManagementSystem\library.db");

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}