using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public LibraryAsset Get(int id)
        => GetAll()
            .FirstOrDefault(asset => asset.Id == id);

        public IEnumerable<LibraryAsset> GetAll()
            => _context.LibraryAssets
            .Include(asset => asset.Status)
            .Include(asset => asset.Location);



        public LibraryBranch GetCurrentLocation(int id)
        => _context
            .LibraryAssets
            .FirstOrDefault(asset => asset.Id == id).Location;

        public string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context
                    .Books
                    .FirstOrDefault(book => book.Id == id).DeweyIndex;
            }

            else return string.Empty;
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context
                    .Books
                    .FirstOrDefault(book => book.Id == id).ISBN;
            }

            else return string.Empty;
        }

        public LibraryCard GetLibraryCardByAssetId(int id)
        {
            throw new System.NotImplementedException();
        }

        public string GetTitle(int id)
        => _context
            .LibraryAssets
            .FirstOrDefault(asset => asset.Id == id).Title;

        public string GetType(int id)
        {
            var book = _context.LibraryAssets.OfType<Book>()
                 .Where(b => b.Id == id);

            return book.Any() ? "Book" : "Video";
        }
        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>()
                .Where(asset => asset.Id == id).Any();

            var isVideo = _context.LibraryAssets.OfType<Video>()
                .Where(asset => asset.Id == id).Any();

            return isBook ?
                _context.Books.FirstOrDefault(book => book.Id == id).Author :
                _context.Videos.FirstOrDefault(vid => vid.Id == id).Director
                ?? "Unknown";     
        }
    }
}
