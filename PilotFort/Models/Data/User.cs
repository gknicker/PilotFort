using System;
using System.Data;

namespace PilotFort.Models.Data
{
	public class User : ICloneable
	{
		public static readonly User NOBODY = new User();

		public User() {
		}

		public User(DataRow row) {
			AspNetUserId = row.Field<string>("Id");
			Email = row.Field<string>("Email");
			EmailConfirmed = row.Field<bool>("EmailConfirmed");
			PasswordHash = row.Field<string>("PasswordHash");
			SecurityStamp = row.Field<string>("SecurityStamp");
			PhoneNumber = row.Field<string>("PhoneNumber");
			PhoneNumberConfirmed = row.Field<bool>("PhoneNumberConfirmed");
			TwoFactorEnabled = row.Field<bool>("TwoFactorEnabled");
			LockoutEndDateUtc = row.Field<DateTime?>("LockoutEndDateUtc");
			LockoutEnabled = row.Field<bool>("LockoutEnabled");
			AccessFailedCount = row.Field<int>("AccessFailedCount");
			UserName = row.Field<string>("UserName");
			Id = row.Field<Guid>("AppUserId");
			FirstName = row.Field<string>("FirstName");
			LastName = row.Field<string>("LastName");
			IsSiteAdmin = row.Field<bool>("IsSiteAdmin");
			CreatedDateUtc = row.Field<DateTime>("CreatedDateUtc");
			CreatedByUserId = row.Field<string>("CreatedByUserId");
			TimeZone = row.Field<string>("TimeZone");
			AccountId = row.Field<Guid?>("AccountId") ?? Guid.Empty;
			IsAccountAdmin = row.Field<bool?>("IsAccountAdmin") ?? false;
		}

		public object Clone() {
			return MemberwiseClone();
		} 

		// AspNetUser attributes
		public string AspNetUserId { get; set; }
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PasswordHash { get; set; }
		public string SecurityStamp { get; set; }
		public string PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public bool TwoFactorEnabled { get; set; }
		public DateTime? LockoutEndDateUtc { get; set; }
		public bool LockoutEnabled { get; set; }
		public int AccessFailedCount { get; set; }
		public string UserName { get; set; }

		// AppUser attributes
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsSiteAdmin { get; set; }
		public DateTime CreatedDateUtc { get; set; }
		public string CreatedByUserId { get; set; }
		public string TimeZone { get; set; }

		// per session attributes
		public Guid AccountId { get; set; }
		public bool IsAccountAdmin { get; set; }
	}
}