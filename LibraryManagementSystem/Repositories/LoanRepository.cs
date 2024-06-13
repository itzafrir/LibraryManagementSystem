using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    public class LoanRepository : IRepository<Loan>
    {
        private readonly LibraryContext _context;

        public LoanRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Loan> GetAll()
        {
            return _context.Loans.Include(l => l.Item).Include(l => l.User).ToList();
        }

        public Loan GetById(int id)
        {
            var loan = _context.Loans.Include(l => l.Item).Include(l => l.User).FirstOrDefault(l => l.Id == id);
            if (loan == null)
                throw new KeyNotFoundException($"Loan with ID {id} not found.");
            return loan;
        }

        public void Add(Loan entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Loans.Add(entity);
            _context.SaveChanges();
        }

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
