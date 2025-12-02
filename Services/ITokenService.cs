using ShopHub.Models;

namespace ShopHub.Services
{    
        public interface ITokenService
        {
            string CreateToken(User user);
        }
    
}
