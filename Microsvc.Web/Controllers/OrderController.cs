using Microsoft.AspNetCore.Mvc;

namespace Microsvc.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult OrderIndex()
        {
            return View();
        }
    }
}
