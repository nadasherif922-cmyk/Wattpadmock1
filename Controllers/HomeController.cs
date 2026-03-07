using Microsoft.AspNetCore.Mvc;

namespace Wattpadmock1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}