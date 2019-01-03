using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PilotFort.Models.Data;

namespace PilotFort.Data
{
	public class LogData
	{
		private static readonly LogData DEFAULT = new LogData();
		public static LogData Default { get { return DEFAULT; } }

		private readonly SqlDb.DB db;

		private LogData() {
			this.db = SqlDb.Default;
		}

		public Guid AddFlightLog(FlightLog model) {
			var cmd = new SqlCommand("dbo.FlightLog_Insert");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@AppUserId", model.UserId);
			cmd.Parameters.AddWithValue("@AircraftId", model.AircraftId);
			cmd.Parameters.AddWithValue("@FlightBeginUtc", model.FlightBeginUtc);
			cmd.Parameters.AddWithValue("@FlightEndUtc", model.FlightEndUtc);
			cmd.Parameters.AddWithValue("@HobbsBegin", model.HobbsBegin);
			cmd.Parameters.AddWithValue("@HobbsEnd", model.HobbsEnd);
			cmd.Parameters.AddWithValue("@HobbsHours", model.HobbsHours);
			cmd.Parameters.AddWithValue("@NightLandings", model.NightLandings);
			cmd.Parameters.AddWithValue("@CreatedByUserId", model.CreatedByUserId);
			object result = db.GetScalar(cmd);
			return (Guid)result;
		}

		public FlightLog GetFlightLog(Guid flightLogId) {
			var cmd = new SqlCommand("dbo.FlightLog_Get");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@FlightLogId", flightLogId);
			DataTable table = db.Get(cmd);
			if (table.Rows.Count == 0) {
				return null;
			}
			return new FlightLog(table.Rows[0]);
		}

		public List<FlightLog> FindFlightLog(Guid? appUserId = null, DateTime? flightDateUtc = null) {
			var results = new List<FlightLog>();
			var cmd = new SqlCommand("dbo.FlightLog_Find");
			cmd.CommandType = CommandType.StoredProcedure;
			if (appUserId != null) {
				cmd.Parameters.AddWithValue("@AppUserId", appUserId.Value);
			}
			if (flightDateUtc != null) {
				cmd.Parameters.AddWithValue("@FlightDateUtc", appUserId.Value);
			}
			DataTable table = db.Get(cmd);
			foreach (DataRow row in table.Rows) {
				var result = new FlightLog(row);
				result.AircraftManufacturer = row.Field<string>("Manufacturer");
				result.AircraftModel = row.Field<string>("Model");
				results.Add(result);
			}
			return results;
		}

		public bool UpdateFlightLog(FlightLog model) {
			var cmd = new SqlCommand("dbo.FlightLog_Update");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@FlightLogId", model.Id);
			cmd.Parameters.AddWithValue("@AppUserId", model.UserId);
			cmd.Parameters.AddWithValue("@AircraftId", model.AircraftId);
			cmd.Parameters.AddWithValue("@FlightBeginUtc", model.FlightBeginUtc);
			cmd.Parameters.AddWithValue("@FlightEndUtc", model.FlightEndUtc);
			cmd.Parameters.AddWithValue("@HobbsBegin", model.HobbsBegin);
			cmd.Parameters.AddWithValue("@HobbsEnd", model.HobbsEnd);
			cmd.Parameters.AddWithValue("@HobbsHours", model.HobbsHours);
			cmd.Parameters.AddWithValue("@NightLandings", model.NightLandings);
			cmd.Parameters.AddWithValue("@CreatedByUserId", model.CreatedByUserId);
			int rowsAffected = db.Execute(cmd);
			bool success = rowsAffected == 1;
			return success;
		}
	}
}