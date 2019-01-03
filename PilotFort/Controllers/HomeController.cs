using System.Web.Mvc;

namespace PilotFort.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index() {
			return View();
		}

		public ActionResult About() {
			ViewBag.Message = "PilotFort description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "PilotFort contact page.";

			return View();
		}
	}
}