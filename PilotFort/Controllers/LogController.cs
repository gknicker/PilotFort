using System;
using System.Web.Mvc;
using PilotFort.Classes;
using PilotFort.Models;

namespace PilotFort.Controllers
{
	public class LogController : Controller
	{
		private readonly Data.LogData logData;
		private readonly Data.HangarData hangarData;

		public LogController() {
			logData = Data.LogData.Default;
			hangarData = Data.HangarData.Default;
		}

		// GET: Log
		[Authorize]
		public ActionResult Index() {
			var model = new FlightLogModel();
			model.Aircraft = hangarData.FindAircraft(Security.UserId);
			model.FlightLog = logData.FindFlightLog(Security.UserId);
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public ActionResult Index(FlightLogModel model) {
			Guid userId = Security.UserId;
			model.CreatedByUserId = userId;
			model.UserId = userId;
			model.FlightBeginUtc = model.FlightBeginUtc.ToUtcTime();
			model.FlightEndUtc = model.FlightEndUtc.ToUtcTime();
			logData.AddFlightLog(model);
			return Index();
		}
	}
}