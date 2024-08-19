using System;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.Utilities.Enums;

public class FineService
{
    private readonly IRepository<Fine> _fineRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Item> _itemRepository;
    private readonly IRepository<Loan> _loanRepository;

    public FineService(IRepository<Fine> fineRepository, IRepository<User> userRepository, IRepository<Item> itemRepository, IRepository<Loan> loanRepository)
    {
        _fineRepository = fineRepository;
        _userRepository = userRepository;
        _itemRepository = itemRepository;
        _loanRepository = loanRepository;
    }

    public void GenerateOrUpdateFines()
    {
        var allLoans = _loanRepository.GetAll().Where(l => l.IsOverdue()).ToList();

        foreach (var loan in allLoans)
        {
            // Check if a fine already exists for this user-item combination
            var existingFine = _fineRepository.GetAll().FirstOrDefault(f => f.UserId == loan.UserId && f.ItemId == loan.ItemId);

            var monthsOverdue = (DateTime.Now - loan.DueDate).Days / Constants.DAYS_PER_FINE_PERIOD;

            if(monthsOverdue > 0)
            { 
                if (existingFine == null)
                {
                    // Fine doesn't exist, create it starting at $1 for the second month
                    var newFine = new Fine
                    {
                        UserId = loan.UserId,
                        ItemId = loan.ItemId,
                        Amount = monthsOverdue * Constants.MONTH_OVERDUE_FINE,
                        DateIssued = DateTime.Now,
                        Status = FineStatus.Unpaid
                    };
                    _fineRepository.Add(newFine);
                }
                else
                {
                    // Fine exists, update the amount based on months overdue
                    existingFine.Amount = monthsOverdue * Constants.MONTH_OVERDUE_FINE;
                    existingFine.DateIssued = DateTime.Now;
                    _fineRepository.Update(existingFine);
                }
            }
        }
    }
}
