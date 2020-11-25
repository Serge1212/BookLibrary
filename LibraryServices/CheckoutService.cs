using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public IEnumerable<Checkout> GetAll()
        => _context.Checkouts;


        public Checkout Get(int id)
        => GetAll().FirstOrDefault(checkout => checkout.Id == id);

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        => _context
            .CheckoutHistories
            .Include(h => h.LibraryAsset)
            .Include(h => h.LibraryCard)
            .Where(h => h.LibraryAsset.Id == id);
            
        public void CheckInItem(int id)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == id);

            //remove any existing checkouts on the item
            RemoveExistingCheckouts(id);
            //close any existing checkout history
            CloseExistingCheckoutHistory(id, now);
            //look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
            //if there are holds, checkout the item to the
            //librarycard with the earliest hold
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(id, currentHolds);
            }
            // otherwice, update the item status to available
            UpdateAssetStatus(id, "Available");
            _context.SaveChanges();
        }

        public void CheckoutItem(int id, int libraryCardId)
        {
             if(IsCheckedOut(id))
            {
                return;
                //add logic here to handle feedback to the user
            }

            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == id);

            UpdateAssetStatus(id, "Checked Out");

            var libraryCard = _context.LibraryCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(l => l.Id == libraryCardId);
            var now = DateTime.Now;
            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        => now.AddDays(30);

        private void CheckoutToEarliestHold(int id, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();

            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckoutItem(id, card.Id);
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
        => _context
            .Holds
            .Include(h => h.LibraryAsset)
            .Where(h => h.LibraryAsset.Id == id);

        public string GetCurrentPatron(int id)
        {
            throw new NotImplementedException();
        }

        public Checkout GetLatestCheckout(int id)
        => _context.Checkouts.Where(c => c.LibraryAsset.Id == id)
            .OrderByDescending(c => c.Since)
            .FirstOrDefault();

        public int GetNumberOfCopies(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsCheckedOut(int id)
        => _context.Checkouts
           .Where(co => co.LibraryAsset.Id == id)
           .Any();
        

        public void MarkFound(int id)
        {
            var now = DateTime.Now;

            UpdateAssetStatus(id, "Available");

            RemoveExistingCheckouts(id);

            CloseExistingCheckoutHistory(id, now);

            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int id, string v)
        {
            var item = _context
               .LibraryAssets
               .FirstOrDefault(a => a.Id == id);

            _context.Update(item);

            item.Status = _context
                .Statuses
                .FirstOrDefault(status => status.Name == "Available");
        }

        private void CloseExistingCheckoutHistory(int id, DateTime now)
        {
            var history = _context
                .CheckoutHistories
                .FirstOrDefault(h => h.LibraryAsset.Id == id
                    && h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int id)
        {
            var checkout = _context
                .Checkouts
                .FirstOrDefault(co => co.LibraryAsset.Id == id);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int id)
        {
            UpdateAssetStatus(id, "Lost");

            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            throw new NotImplementedException();
        }
    }
}
