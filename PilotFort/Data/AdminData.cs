using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using PilotFort.Classes;
using PilotFort.Models.Data;

namespace PilotFort.Data
{
	public class AdminData
	{
		private static readonly AdminData DEFAULT = new AdminData();
		public static AdminData Default { get { return DEFAULT; } }

		private static string LogPath {
			get { return System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/errorLog.txt"); }
		}

		private readonly SqlDb.DB db;

		private AdminData() {
			this.db = SqlDb.Default;
		}

		public int LogEmail(Email email) {
			var cmd = new SqlCommand("add_Email");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@From", email.From.Truncate(100));
			cmd.Parameters.AddWithValue("@To", email.To.Truncate(300));
			cmd.Parameters.AddWithValue("@Subject", email.Subject.Truncate(300));
			cmd.Parameters.AddWithValue("@Body", email.Message.Truncate(3000));
			cmd.Parameters.AddWithValue("@Error", ((object)email.Error ?? string.Empty).ToString().Truncate(300));
			cmd.Parameters.AddWithValue("@UserName", email.UserName.Truncate(30));
			return this.db.Execute(cmd);
		}

		public Guid LogError(Exception x, string userName = "") {
			string errorClass = x.GetType().ToString();
			string errorMessage = x.Message ?? string.Empty;
			var stackTrace = new StringBuilder(x.StackTrace ?? string.Empty);
			if (x is AggregateException) {
				IEnumerable<Exception> innerExceptions = ((AggregateException)x).Flatten().InnerExceptions;
				foreach (Exception ex in innerExceptions) {
					errorClass += ": " + ex.GetType();
					errorMessage += ": " + ex.Message;
					stackTrace.AppendLine().AppendLine().Append(ex.StackTrace);
					break;
				}
			}
			return LogError(errorClass, errorMessage, stackTrace.ToString(), userName);
		}

		public Guid LogError(string errorClass, string errorMessage, string stackTrace, string userName = "") {
			Guid errorId = Guid.NewGuid();
			try {
				var cmd = new SqlCommand("AppError_Insert");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@AppErrorId", errorId);
				cmd.Parameters.AddWithValue("@ErrorClass", errorClass.Truncate(255));
				cmd.Parameters.AddWithValue("@ErrorMessage", errorMessage.Truncate(255));
				cmd.Parameters.AddWithValue("@StackTrace", stackTrace.Truncate(1024));
				if (!string.IsNullOrWhiteSpace(userName)) {
					cmd.Parameters.AddWithValue("@UserName", userName.Truncate(128));
				}
			} catch (SqlException x) {
				LogDebug(x);
				errorId = Guid.Empty;
			}
			return errorId;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Guid LogError(string message, string userName = "", Exception cause = null) {
			//Captures 1 frame, false for not collecting information about the file
			var callerTrace = new StackTrace(1, false);
			MethodBase caller = callerTrace.GetFrame(0).GetMethod();
			string errorClass = cause == null ? caller.DeclaringType.FullName : cause.GetType().FullName;
			string errorMessage = cause == null ? message : message + Environment.NewLine + cause.Message;
			string stackTrace = cause == null ? caller.Name : caller.Name + Environment.NewLine + cause.StackTrace;
			return LogError(errorClass, errorMessage, stackTrace, userName);
		}

		public void LogDebug(Exception x) {
			if (x != null && Application.NotProduction) {
				using (System.IO.StreamWriter writer = System.IO.File.AppendText(LogPath)) {
					writer.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					writer.Write(" - ");
					writer.Write(x.GetType().Name);
					writer.Write(": ");
					writer.WriteLine(x.Message);
					writer.WriteLine(x.StackTrace);
					writer.WriteLine();
				}
			}
		}

		public void LogDebug(string message) {
			if (message != null && Application.NotProduction) {
				using (System.IO.StreamWriter writer = System.IO.File.AppendText(LogPath)) {
					writer.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					writer.Write(" - ");
					writer.WriteLine(message);
				}
			}
		}
	}
}