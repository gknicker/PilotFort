using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PilotFort.Models.Data;

namespace PilotFort.Data
{
	public class AccountData
	{
		private static readonly AccountData DEFAULT = new AccountData();
		public static AccountData Default { get { return DEFAULT; } }

		private readonly SqlDb.DB db;

		private AccountData() {
			this.db = SqlDb.Default;
		}

		public int CreateAppUser(string aspNetUserName) {
			var cmd = new SqlCommand("AppUser_Insert");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@UserName", aspNetUserName);
			return db.Execute(cmd);
		}

		public User GetUser(Guid appUserId) {
			var cmd = new SqlCommand("AppUser_Get");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@AppUserId", appUserId);
			DataTable table = db.Get(cmd);
			if (table.Rows.Count == 0) {
				return null;
			}
			return new User(table.Rows[0]);
		}

		public User GetUser(string userName) {
			var cmd = new SqlCommand("AppUser_GetByUserName");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@UserName", userName);
			DataTable table = db.Get(cmd);
			if (table.Rows.Count == 0) {
				return null;
			}
			return new User(table.Rows[0]);
		}

		public List<Account> GetUserAccounts(Guid appUserId) {
			var results = new List<Account>();
			var cmd = new SqlCommand("Account_FindByUser");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@AppUserId", appUserId);
			DataTable table = db.Get(cmd);
			foreach (DataRow row in table.Rows) {
				results.Add(new Account(row));
			}
			return results;
		}
	}
}