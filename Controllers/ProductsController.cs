using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Models;
using ShopHub.Services;

namespace ShopHub.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MongoDbService _mongoService;
        public ProductsController(MongoDbService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<IActionResult> Index(string searchQuery, string category)
        {
     
            if (string.IsNullOrEmpty(searchQuery) && (string.IsNullOrEmpty(category) || category == "All category"))
            {
                var products = await _mongoService.SearchProductsAsync(searchQuery, category);
                return View(products);
            }
            else
            {
                var products = await _mongoService.SearchProductsAsync(searchQuery, category);
                return View(products);
            }
        }

        public async Task<IActionResult> Details(string id) 
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var product = await _mongoService.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> SeedData()
        {
            var existing = await _mongoService.GetProductsAsync();
            if (existing.Count == 0)
            {
                var sampleProducts = new List<ProductViewModel>
                {
                    new ProductViewModel { Name = "Canon Camera EOS 2000", Price = 998.00m, Category = "Electronics", ImageUrl = "https://via.placeholder.com/300", ShortDescription = "High quality camera...", Rating = 4.5, ReviewsCount = 154, InStock = true },
                    new ProductViewModel { Name = "GoPro HERO6 4K", Price = 400.00m, Category = "Electronics", ImageUrl = "https://via.placeholder.com/300", ShortDescription = "Action camera...", Rating = 5.0, ReviewsCount = 100, InStock = true },
                    new ProductViewModel { Name = "Men's T-Shirt", Price = 15.00m, Category = "Clothing", ImageUrl = "https://via.placeholder.com/300", ShortDescription = "Cotton shirt...", Rating = 3.5, ReviewsCount = 20, InStock = true }
                };

                foreach (var p in sampleProducts)
                {
                    await _mongoService.CreateProductAsync(p);
                }
                return Content("Database seeded successfully!");
            }
            return Content("Database already has data.");
        }

        [Authorize] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _mongoService.CreateProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}