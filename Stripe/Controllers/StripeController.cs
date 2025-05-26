using Microsoft.AspNetCore.Mvc;

namespace Stripe.Controllers
{
    public class StripeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
