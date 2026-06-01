using Microsoft.AspNetCore.Mvc;

namespace ITMRestaurant.API.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
