﻿using System.Collections.Generic;

namespace BookLibrary.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> Assets { get; set; }
    }
}
