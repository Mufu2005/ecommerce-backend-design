using Microsoft.AspNetCore.Mvc;

namespace ShopHub.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
