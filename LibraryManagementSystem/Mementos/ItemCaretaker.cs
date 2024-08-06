using System;
using System.Collections.Generic;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Mementos
{
    public class ItemCaretaker
    {
        private readonly Stack<ItemMemento> _mementos = new();

        public void SaveState(Item item)
        {
            _mementos.Push(new ItemMemento(item));
        }

        public void Undo(Item item)
        {
            if (_mementos.Count > 0)
            {
                ItemMemento memento = _mementos.Pop();
                memento.Restore(item);
            }
        }

        public bool CanUndo() => _mementos.Count > 0;
    }

    public class ItemMemento
    {
        public int Id { get; }
        public string Title { get; }
        public string ISBN { get; }
        public ItemType ItemType { get; }
        public double Rating { get; }
        public DateTime PublicationDate { get; }
        public string Publisher { get; }
        public string Description { get; }

        public ItemMemento(Item item)
        {
            Id = item.Id;
            Title = item.Title;
            ISBN = item.ISBN;
            Rating = item.Rating;
            PublicationDate = item.PublicationDate;
            Publisher = item.Publisher;
            Description = item.Description;
        }

        public void Restore(Item item)
        {
            item.Id = Id;
            item.Title = Title;
            item.ISBN = ISBN;
            //item.ItemType = ItemType;
            item.Rating = Rating;
            item.PublicationDate = PublicationDate;
            item.Publisher = Publisher;
            item.Description = Description;
        }
    }
}