using System.Data;

namespace PilotFort.Models.Data
{
	public class Aircraft
	{
		public Aircraft() {
		}

		public Aircraft(DataRow row) {
			Id = row.Field<string>("AircraftId");
			SerialNumber = row.Field<string>("SerialNumber");
			ModelId = row.Field<string>("AircraftModelId");
			YearManufactured = (short)row.Field<decimal>("YearManufactured");
			Manufacturer = row.Field<string>("Manufacturer");
			Model = row.Field<string>("Model");
		}

		public string Id { get; set; }
		public string SerialNumber { get; set; }
		public string ModelId { get; set; }
		public short YearManufactured { get; set; }

		// additional attributes displayed in UI
		public string Manufacturer { get; set; }
		public string Model { get; set; }
		public string Display {
			get { return Id + " (" + YearManufactured + " " + Manufacturer + " " + Model + ")"; }
		}
	}
}