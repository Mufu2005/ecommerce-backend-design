using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Models;
using ShopHub.Models; // Update to your actual namespace

namespace ShopHub.Controllers
{
    public class ProductsController : Controller
    {
        // TEMPORARY: Mock Data to make the UI work immediately without a Database.
        // In Step 3, we will replace this with your actual Database context.
        private static readonly List<ProductViewModel> _products = new List<ProductViewModel>
        {
            new ProductViewModel {
                Id = 1,
                Name = "Canon Camera EOS 2000, Black 10x zoom",
                Price = 998.00m,
                OldPrice = 1128.00m,
                Rating = 4.5,
                Category = "Electronics",
                ImageUrl = "https://via.placeholder.com/300",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                FullDescription = "<p>Full details about the Canon camera...</p>",
                InStock = true,
                ReviewsCount = 154
            },
            new ProductViewModel {
                Id = 2,
                Name = "GoPro HERO6 4K Action Camera - Black",
                Price = 998.00m,
                Rating = 5.0,
                Category = "Electronics",
                ImageUrl = "https://via.placeholder.com/300",
                ShortDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco.",
                FullDescription = "<p>Full details about GoPro...</p>",
                InStock = true,
                ReviewsCount = 102
            },
            new ProductViewModel {
                Id = 3,
                Name = "Mens Long Sleeve T-shirt Cotton Base Layer",
                Price = 98.00m,
                Category = "Clothing",
                ImageUrl = "https://via.placeholder.com/300",
                ShortDescription = "Great for winter.",
                FullDescription = "<p>Cotton material, very comfortable...</p>",
                InStock = true,
                ReviewsCount = 32
            }
        };

        // GET: /Products?searchQuery=camera&category=Electronics
        public IActionResult Index(string searchQuery, string category)
        {
            var products = _products.AsQueryable();

            // 1. Handle Search Logic (Week 2 Req)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase));
            }

            // 2. Handle Category Filtering
            if (!string.IsNullOrEmpty(category) && category != "All category")
            {
                products = products.Where(p => p.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase));
            }

            return View(products.ToList());
        }

        // GET: /Products/Details/5
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}