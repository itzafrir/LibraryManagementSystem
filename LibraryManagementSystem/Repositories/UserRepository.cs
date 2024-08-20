using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    /// <summary>
    /// Provides repository methods for managing users in the library system, including CRUD operations.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="LibraryContext"/> used to interact with the database.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided context is null.</exception>
        public UserRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{User}"/> containing all users.</returns>
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The <see cref="User"/> with the specified ID, or null if not found.</returns>
        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="entity">The <see cref="User"/> to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided user entity is null.</exception>
        public void Add(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="entity">The <see cref="User"/> to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided user entity is null.</exception>
        public void Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when the user with the specified ID is not found.</exception>
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
