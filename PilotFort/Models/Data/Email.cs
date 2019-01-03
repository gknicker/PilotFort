using System;
using System.Net.Mail;

namespace PilotFort.Models.Data
{
	public class Email
	{
		public string Subject { get; set; }
		public string Message { get; set; }
		public string To { get; set; }
		/// <summary>Recipient, will be defaulted to value in Web.config file if left blank.</summary>
		public string From { get; set; }

		public Attachment Attachment { get; set; }

		/// <summary>For internal use only, user name of current user, for logging.</summary>
		public string UserName { get; set; }

		/// <summary>For internal use only, exception trying to send email, for logging.</summary>
		public Exception Error { get; set; }
	}
}