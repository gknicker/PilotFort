using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PilotFort.Models.Data;

namespace PilotFort.Data
{
	public class HangarData
	{
		private static readonly HangarData DEFAULT = new HangarData();
		public static HangarData Default { get { return DEFAULT; } }

		private readonly SqlDb.DB db;

		private HangarData() {
			this.db = SqlDb.Default;
		}

		public Aircraft GetAircraft(string aircraftId) {
			var cmd = new SqlCommand("dbo.Aircraft_Get");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@AircraftId", aircraftId);
			DataTable table = db.Get(cmd);
			if (table.Rows.Count == 0) {
				return null;
			}
			return new Aircraft(table.Rows[0]);
		}

		public List<Aircraft> FindAircraft(Guid? appUserId = null) {
			var results = new List<Aircraft>();
			var cmd = new SqlCommand("dbo.Aircraft_Find");
			cmd.CommandType = CommandType.StoredProcedure;
			if (appUserId != null) {
				cmd.Parameters.AddWithValue("@AppUserId", appUserId.Value);
			}
			DataTable table = db.Get(cmd);
			foreach (DataRow row in table.Rows) {
				var result = new Aircraft(row);
				results.Add(result);
			}
			return results;
		}
	}
}