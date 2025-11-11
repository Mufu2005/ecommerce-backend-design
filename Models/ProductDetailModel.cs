using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopHub.Models
{
    public class ProductDetailModel : PageModel
    {
        public int? Id { get; set; }

        public void OnGet(int? id)
        {
            Id = id;
        }
    }
}
