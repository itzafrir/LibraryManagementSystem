using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly LibraryContext _context;

        public UserRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Add(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Users.Find(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Remove(entity);
            _context.SaveChanges();
        }
    }
}
