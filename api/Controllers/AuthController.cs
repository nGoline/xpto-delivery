using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/") {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}