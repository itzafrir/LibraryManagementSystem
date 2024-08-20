using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    /// <summary>
    /// Provides repository methods for managing items in the library, including CRUD operations.
    /// </summary>
    public class ItemRepository : IRepository<Item>
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="LibraryContext"/> used to interact with the database.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided context is null.</exception>
        public ItemRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all items from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Item}"/> containing all items.</returns>
        public IEnumerable<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        /// <summary>
        /// Retrieves an item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item.</param>
        /// <returns>The <see cref="Item"/> with the specified ID, or null if not found.</returns>
        public Item GetById(int id)
        {
            return _context.Items.Find(id);
        }

        /// <summary>
        /// Adds a new item to the database.
        /// </summary>
        /// <param name="entity">The <see cref="Item"/> to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided item is null.</exception>
        public void Add(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Items.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing item in the database.
        /// </summary>
        /// <param name="entity">The <see cref="Item"/> to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided item is null.</exception>
        public void Update(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Items.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes an item from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when the item with the specified ID is not found.</exception>
        public void Delete(int id)
        {
            var entity = _context.Items.Find(id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Items.Remove(entity);
            _context.SaveChanges();
        }
    }
}
