using System.Collections.Generic;
using PilotFort.Models.Data;

namespace PilotFort.Models
{
	public class FlightLogModel : FlightLog
	{
		public List<Aircraft> Aircraft { get; set; }
		public List<FlightLog> FlightLog { get; set; }
	}
}