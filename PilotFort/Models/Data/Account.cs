using System;
using System.Data;

namespace PilotFort.Models.Data
{
	public class Account
	{
		public Account() {
		}

		public Account(DataRow row) {
			Id = row.Field<Guid>("AccountId");
			Name = row.Field<string>("AccountName");
			CreatedDateUtc = row.Field<DateTime>("CreatedDateUtc");
			CreatedByUserId = row.Field<Guid>("CreatedByUserId");
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDateUtc { get; set; }
		public Guid CreatedByUserId { get; set; }
	}
}