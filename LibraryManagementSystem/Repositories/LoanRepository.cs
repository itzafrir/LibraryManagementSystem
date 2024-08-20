using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    /// <summary>
    /// Provides repository methods for managing loans in the library, including CRUD operations.
    /// </summary>
    public class LoanRepository : IRepository<Loan>
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoanRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="LibraryContext"/> used to interact with the database.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided context is null.</exception>
        public LoanRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all loans from the database, including associated items and users.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Loan}"/> containing all loans.</returns>
        public IEnumerable<Loan> GetAll()
        {
            return _context.Loans.Include(l => l.Item).Include(l => l.User).ToList();
        }

        /// <summary>
        /// Retrieves a loan by its unique identifier, including associated item and user.
        /// </summary>
        /// <param name="id">The unique identifier of the loan.</param>
        /// <returns>The <see cref="Loan"/> with the specified ID, or throws a <see cref="KeyNotFoundException"/> if not found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a loan with the specified ID is not found.</exception>
        public Loan GetById(int id)
        {
            var loan = _context.Loans.Include(l => l.Item).Include(l => l.User).FirstOrDefault(l => l.Id == id);
            if (loan == null)
                throw new KeyNotFoundException($"Loan with ID {id} not found.");
            return loan;
        }

        /// <summary>
        /// Adds a new loan to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Loan"/> to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided loan entity is null.</exception>
        public void Add(Loan entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Loans.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing loan in the database.
        /// </summary>
        /// <param name="entity">The <see cref="Loan"/> to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided loan entity is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when a loan with the specified ID is not found.</exception>
        public void Update(Loan entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!_context.Loans.Any(l => l.Id == entity.Id))
                throw new KeyNotFoundException($"Loan with ID {entity.Id} not found.");

            _context.Loans.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a loan from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the loan to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when a loan with the specified ID is not found.</exception>
        public void Delete(int id)
        {
            var entity = _context.Loans.Find(id);
            if (entity == null)
                throw new KeyNotFoundException($"Loan with ID {id} not found.");

            _context.Loans.Remove(entity);
            _context.SaveChanges();
        }
    }
}
