using System;

namespace PilotFort.Models.Data
{
	/// <summary>
	/// For calling sunrise-sunset.org API with formatted = 0, wherein
	/// times are returned in the format 2015-05-21T05:05:35+00:00.
	/// <list type="bullet">
	/// <listheader><description>Statuses</description></listheader>
	/// <item><term>OK</term><description>indicates that no errors occurred</description></item>
	/// <item><term>INVALID_REQUEST</term><description>indicates that either lat or lng parameters are missing or invalid</description></item>
	/// <item><term>INVALID_DATE</term><description>indicates that date parameter is missing or invalid</description></item>
	/// <item><term>UNKNOWN_ERROR</term><description>indicates that the request could not be processed due to a server error. The request may succeed if you try again.</description></item>
	/// </list>
	/// </summary>
	public class SunriseSunset
	{
		public SunriseSunsetResults results { get; set; }
		public string status { get; set; }

		// interpretation
		public bool IsSuccessful { get { return string.Equals("OK", status); } }
		public int DayLengthSeconds { get { return results.day_length; } }
	}

	public class SunriseSunsetResults
	{
		public string sunrise { get; set; }
		public string sunset { get; set; }
		public string solar_noon { get; set; }
		public int day_length { get; set; }
		public string civil_twilight_begin { get; set; }
		public string civil_twilight_end { get; set; }
		public string nautical_twilight_begin { get; set; }
		public string nautical_twilight_end { get; set; }
		public string astronomical_twilight_begin { get; set; }
		public string astronomical_twilight_end { get; set; }
	}
}