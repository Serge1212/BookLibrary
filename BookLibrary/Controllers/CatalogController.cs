using BookLibrary.Models.Catalog;
using LibraryData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookLibrary.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _assets;
        public CatalogController(ILibraryAsset assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();
            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.Get(id);

            //var currentHolds = _checkoutsService.GetCurrentHolds(id).Select(a => new AssetHoldModel
            //{
            //    HoldPlaced = _checkoutsService.GetCurrentHoldPlaced(a.Id),
            //    PatronName = _checkoutsService.GetCurrentHoldPatron(a.Id)
            //});

            //var model = new AssetDetailModel
            //{
            //    AssetId = id,
            //    Title = asset.Title,
            //    Type = _assets.GetType(id),
            //    Year = asset.Year,
            //    Cost = asset.Cost,
            //    Status = asset.Status.Name,
            //    ImageUrl = asset.ImageUrl,
            //    AuthorOrDirector = _assets.GetAuthorOrDirector(id),
            //    CurrentLocation = _assets.GetCurrentLocation(id)?.Name,
            //    Dewey = _assets.GetDeweyIndex(id),
            //    CheckoutHistory = _assets.GetCheckoutHistory(id),
            //    CurrentAssociatedLibraryCard = _assets.GetLibraryCardByAssetId(id),
            //    Isbn = _assetsService.GetIsbn(id),
            //    LatestCheckout = _checkoutsService.GetLatestCheckout(id),
            //    CurrentHolds = currentHolds,
            //    PatronName = _checkoutsService.GetCurrentPatron(id)
            //};

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                DeweyCallNumber = _assets.GetDeweyIndex(id),
                ISBN = _assets.GetIsbn(id)
            };

            return View(model);
        }
    }
}
