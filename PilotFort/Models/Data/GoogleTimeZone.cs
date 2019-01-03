using System;

namespace PilotFort.Models.Data
{
	public class GoogleTimeZone
	{
		public int dstOffset { get; set; }
		public int rawOffset { get; set; }
		public string status { get; set; }
		public string timeZoneId { get; set; }
		public string timeZoneName { get; set; }

		// interpretation
		public int DaylightSavingsOffsetSeconds { get { return dstOffset; } }
		public int RawOffsetSeconds { get { return rawOffset; } }
		public bool IsSuccessful { get { return string.Equals("OK", status); } }
		public string ErrorMessage { get { return IsSuccessful ? string.Empty : status; } }
	}
}