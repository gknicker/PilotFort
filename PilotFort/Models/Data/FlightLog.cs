using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using PilotFort.Classes;

namespace PilotFort.Models.Data
{
	public class FlightLog
	{
		public FlightLog() {
		}

		public FlightLog(DataRow row) {
			Id = row.Field<Guid>("FlightLogId");
			UserId = row.Field<Guid>("AppUserId");
			AircraftId = row.Field<string>("AircraftId");
			FlightBeginUtc = row.Field<DateTime?>("FlightBeginUtc");
			FlightEndUtc = row.Field<DateTime?>("FlightEndUtc");
			HobbsBegin = row.Field<decimal?>("HobbsBegin");
			HobbsEnd = row.Field<decimal?>("HobbsEnd");
			HobbsHours = row.Field<decimal?>("HobbsHours");
			NightLandings = row.Field<byte>("NightLandings");
			CreatedDateUtc = row.Field<DateTime?>("CreatedDateUtc");
			CreatedByUserId = row.Field<Guid>("CreatedByUserId");
		}

		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		[Required]
		[Display(Name = "Aircraft ID")]
		public string AircraftId { get; set; }
		public DateTime? FlightBeginUtc { get; set; }
		public DateTime? FlightBegin { get { return FlightBeginUtc.ToUserTimeZone(); } }
		public DateTime? FlightEndUtc { get; set; }
		public DateTime? FlightEnd { get { return FlightEndUtc.ToUserTimeZone(); } }
		[Display(Name = "Hobbs Begin")]
		public decimal? HobbsBegin { get; set; }
		[Display(Name = "Hobbs End")]
		public decimal? HobbsEnd { get; set; }
		[Display(Name = "Hours")]
		public decimal? HobbsHours { get; set; }
		[Display(Name = "Night Landings")]
		public byte NightLandings { get; set; }
		public DateTime? CreatedDateUtc { get; set; }
		public DateTime? CreatedDate { get { return CreatedDateUtc.ToUserTimeZone(); } }
		public Guid CreatedByUserId { get; set; }

		// additional data attributes for UI date input
		public DateTime FlightBeginDate { get; set; }
		[Required]
		[Display(Name = "Flight Date")]
		public DateTime? FlightDate { get { return FlightBeginDate.ToNullable(); } set { FlightBeginDate = value.ToValue(); } }
		[Display(Name = "Begin Time")]
		[RegularExpression(@"^((0?[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)|(0?[1-9]|1[1-9]|2[0-3]):[0-5][0-9])$", ErrorMessage = "Invalid Time")]
		public string FlightBeginTime {
			get { return FlightBegin.ToTimeDisplay(); }
			set { FlightBeginUtc = FlightBeginDate.Date.Add(DateTime.Parse(value).TimeOfDay); }
		}
		[Display(Name = "End Time")]
		[RegularExpression(@"^((0?[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)|(0?[1-9]|1[1-9]|2[0-3]):[0-5][0-9])$", ErrorMessage = "Invalid Time")]
		public string FlightEndTime {
			get { return FlightEnd.ToTimeDisplay(); }
			set {
				FlightEndUtc = FlightBeginDate.Date.Add(DateTime.Parse(value).TimeOfDay);
				if (FlightBeginUtc > FlightEndUtc) FlightEndUtc = FlightEndUtc.Value.AddDays(1);
			}
		}

		// additional data attributes for displaying in flight log
		[Display(Name = "Flight Begin")]
		public string FlightBeginDisplay { get { return FlightBegin.ToDateTimeDisplay(); } }
		[Display(Name = "Flight End")]
		public string FlightEndDisplay { get { return FlightEnd.ToDateTimeDisplay(); } }
		[Display(Name = "Manufacturer")]
		public string AircraftManufacturer { get; set; }
		[Display(Name = "Model")]
		public string AircraftModel { get; set; }
		[Display(Name = "Created")]
		public string CreatedDateDisplay { get { return CreatedDate.ToDateTimeDisplay(); } }
	}
}