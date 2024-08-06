using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LibraryContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingEntity = _dbSet.Find(GetEntityKey(entity));
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Entity does not exist in the database.");
            }
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
        private object GetEntityKey(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(p => p.PropertyInfo.GetValue(entity))
                .Single();

            return key;
        }
    }
}