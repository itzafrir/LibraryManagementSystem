using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Repositories
{
    /// <summary>
    /// A generic repository class for performing CRUD operations on any entity type within the library management system.
    /// </summary>
    /// <typeparam name="T">The entity type managed by this repository.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LibraryContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="LibraryContext"/> used to interact with the database.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided context is null.</exception>
        public Repository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all entities of type <typeparamref name="T"/>.</returns>
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity with the specified ID, or null if not found.</returns>
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided entity is null.</exception>
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided entity is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity does not exist in the database.</exception>
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

        /// <summary>
        /// Deletes an entity from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when the entity with the specified ID is not found.</exception>
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves the primary key value of the given entity.
        /// </summary>
        /// <param name="entity">The entity whose key is to be retrieved.</param>
        /// <returns>The primary key value of the entity.</returns>
        private object GetEntityKey(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(p => p.PropertyInfo.GetValue(entity))
                .Single();

            return key;
        }
    }
}
