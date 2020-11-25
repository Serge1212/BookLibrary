using LibraryData;
using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;
        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public void CheckInItem(int id)
        {
            throw new NotImplementedException();
        }

        public void CheckoutItem(int id, int libraryCardId)
        {
            throw new NotImplementedException();
        }

        public Checkout Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Checkout> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentHoldPatron(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentHoldPlaced(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentPatron(int id)
        {
            throw new NotImplementedException();
        }

        public Checkout GetLatestCheckout(int id)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfCopies(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsCheckedOut(int id)
        {
            throw new NotImplementedException();
        }

        public void MarkFound(int id)
        {
            throw new NotImplementedException();
        }

        public void MarkLost(int id)
        {
            throw new NotImplementedException();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }
    }
}
