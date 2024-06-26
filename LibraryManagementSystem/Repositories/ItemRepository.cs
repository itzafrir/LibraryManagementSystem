﻿using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly LibraryContext _context;

        public ItemRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        public Item GetById(int id)
        {
            return _context.Items.Find(id);
        }

        public void Add(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Items.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Items.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

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