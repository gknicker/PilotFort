using System.Configuration;

namespace PilotFort.Classes
{
	public static class Application
	{
		public static string Name {
			get { return ConfigurationManager.AppSettings["ApplicationName"]; }
		}

		public static string HostName {
			get { return ConfigurationManager.AppSettings["ApplicationHostName"]; }
		}

		public static string EnvironmentName {
			get { return ConfigurationManager.AppSettings["ApplicationEnvironment"]; }
		}

		public static bool IsProduction {
			get { return string.Equals("Prod", EnvironmentName); }
		}

		public static bool NotProduction {
			get { return !IsProduction; }
		}
	}
}