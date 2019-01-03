using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PilotFort.Data
{
	public class SqlDb
	{
		public static DB Default { get { return DEFAULT; } }
		private static readonly DB DEFAULT = new DB(ConnectionString);

		public static string ConnectionString {
			get { return ConfigurationManager.ConnectionStrings["PilotFort"].ConnectionString; }
		}

		//It is intended that only the DAL classes call this helper class
		public class DataAccessException : ApplicationException
		{
			public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
		}
		public class DB
		{
			private string connectionString;
			public DB(string connectionString) {
				this.connectionString = connectionString;
			}
			public string ConnectionString {
				get { return this.connectionString; }
			}
			public DataTable Get(string sql) {
				SqlCommand cmd = null;
				try {
					using (SqlConnection conn = new SqlConnection(connectionString)) {
						using (cmd = new SqlCommand(sql)) {
							cmd.Connection = conn;
							conn.Open();
							using (SqlDataAdapter a = new SqlDataAdapter(cmd)) {
								DataTable table = new DataTable();
								a.Fill(table);
								return table;
							}
						}
					}
				} catch (Exception x) {
					throw CreateException(x, cmd);
				}
			}
			public DataTable Get(SqlCommand cmd, bool rawSql = false) {
				try {
					using (SqlConnection conn = new SqlConnection(connectionString)) {
						if (!rawSql)
							cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = 360;
						cmd.Connection = conn;
						conn.Open();
						using (SqlDataAdapter a = new SqlDataAdapter(cmd)) {
							DataTable table = new DataTable();
							a.Fill(table);
							return table;
						}
					}
				} catch (Exception x) {
					throw CreateException(x, cmd);
				}
			}

			public DataTable Get(SqlCommand cmd, int pageNumber, int resultsPerPage, out int totalResults) {
				var table = new DataTable();
				using (var conn = new SqlConnection(this.connectionString)) {
					conn.Open();
					cmd.Connection = conn;
					using (var adapter = new SqlDataAdapter(cmd))
						adapter.Fill((pageNumber - 1) * resultsPerPage, resultsPerPage, table);
					using (var count = new SqlCommand("select @@ROWCOUNT", conn))
						totalResults = Convert.ToInt32(count.ExecuteScalar());
				}
				return table;
			}

			public DataSet GetDataSet(SqlCommand cmd) {
				try {
					using (SqlConnection conn = new SqlConnection(connectionString)) {
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Connection = conn;
						conn.Open();
						using (SqlDataAdapter a = new SqlDataAdapter(cmd)) {
							DataSet set = new DataSet();
							a.Fill(set);
							return set;
						}
					}
				} catch (Exception x) {
					throw CreateException(x, cmd);
				}
			}
			public object GetScalar(SqlCommand cmd, bool rawSql = false) {
				try {
					using (SqlConnection conn = new SqlConnection(connectionString)) {
						if (!rawSql)
							cmd.CommandType = CommandType.StoredProcedure;
						cmd.Connection = conn;
						conn.Open();
						object o = cmd.ExecuteScalar();
						if (o is DBNull)
							return null; //So calling code only needs to worry about null, not DBNull
						return o;
					}
				} catch (Exception x) {
					throw CreateException(x, cmd);
				}
			}
			public int Execute(SqlCommand cmd, bool rawSql = false) {
				try {
					using (SqlConnection conn = new SqlConnection(connectionString)) {
						if (!rawSql)
							cmd.CommandType = CommandType.StoredProcedure;
						cmd.Connection = conn;
						conn.Open();
						return cmd.ExecuteNonQuery();
					}
				} catch (Exception x) {
					throw CreateException(x, cmd);
				}
			}
			private Exception CreateException(Exception x, SqlCommand cmd) {
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(x.Message);

				sb.AppendLine("Command: " + cmd.CommandText);
				foreach (SqlParameter p in cmd.Parameters)
					sb.AppendLine(" " + p.ParameterName + " = " + p.Value);

				if (x is SqlException) {
					SqlException sx = (SqlException)x;
					foreach (SqlError error in sx.Errors) {
						sb.AppendLine("Error: " + error.Number + "  " + error.Message);
						sb.AppendLine(" on Server: " + error.Server);
						sb.AppendLine(" in Procedure: " + error.Procedure);
						sb.AppendLine(" at Line: " + error.LineNumber);
					}
				}
				return new DataAccessException(sb.ToString(), x);
			}
		}
	}
}