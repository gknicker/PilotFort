using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using PilotFort.Classes;
using PilotFort.Models.Data;

namespace PilotFort.Data
{
	public class WebData
	{
		public static string GoogleApiKey {
			get { return ConfigurationManager.AppSettings["GoogleApiKey"]; }
		}

		public GoogleTimeZone GetTimeZone(decimal latitude, decimal longitude, DateTime? utcDate = null) {
			DateTime dateTime = utcDate == null ? DateTime.UtcNow : utcDate.Value;
			int unixTimestamp = dateTime.ToUnixTimestamp();
			var url = new StringBuilder("https://maps.googleapis.com/maps/api/timezone/json");
			url.Append("?location=").Append(latitude).Append(",").Append(longitude);
			url.Append("&timestamp=").Append(unixTimestamp);
			url.Append("&key=").Append(GoogleApiKey);
			string json = GET(url.ToString());
			return json.Deserialize<GoogleTimeZone>();
		}

		public SunriseSunset GetSunriseSunset(decimal latitude, decimal longitude, DateTime? date = null) {
			var url = new StringBuilder("http://api.sunrise-sunset.org/json");
			url.Append("?lat=").Append(latitude);
			url.Append("&lng=").Append(longitude);
			url.Append("&date=").Append(date == null ? "today" : date.Value.ToString("yyyy-MM-dd"));
			url.Append("&formatted=0");
			string json = GET(url.ToString());
			return json.Deserialize<SunriseSunset>();
		}

		/// <summary>Returns JSON string (or whatever raw response body from the web service call).</summary>
		/// <param name="url">A web service URL</param>
		/// <returns>The raw response body from the web service call, presumably a JSON string</returns>
		string GET(string url) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			try {
				WebResponse response = request.GetResponse();
				using (Stream responseStream = response.GetResponseStream()) {
					StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
					return reader.ReadToEnd();
				}
			} catch (WebException ex) {
				WebResponse errorResponse = ex.Response;
				if (errorResponse == null) {
					AdminData.Default.LogError(ex, Security.UserName);
				} else
					using (Stream responseStream = errorResponse.GetResponseStream()) {
						StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
						String errorText = reader.ReadToEnd();
						AdminData.Default.LogError(errorText, Security.UserName, ex);
					}
				throw;
			}
		}
	}
}