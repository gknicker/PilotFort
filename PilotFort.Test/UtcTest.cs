using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZoneNames;

namespace PilotFort.Test
{
	[TestClass]
	public class UtcTest
	{
		[TestMethod]
		public void TestLocalToUniversalToLocal() {
			DateTime localTime = new DateTime(2017, 5, 1, 12, 0, 0);
			DateTimeOffset localOffset = new DateTimeOffset(localTime);
			DateTimeOffset zeroOffset = localOffset.ToOffset(TimeSpan.Zero);
			Assert.AreEqual(localOffset, zeroOffset, "local offset equals zero offset");
			Assert.IsFalse(localOffset.EqualsExact(zeroOffset), "local offset exactly equals zero offset");

			DateTimeOffset universalOffset = localOffset.ToUniversalTime();
			DateTime universalTime = universalOffset.UtcDateTime;
			Assert.AreNotEqual(localTime, universalTime, "local time does not equal universal time");
			Assert.AreEqual(zeroOffset, universalOffset, "zero offset equals universal offset");
			Assert.IsTrue(zeroOffset.EqualsExact(universalOffset), "zero offset exactly equals universal offset");

			universalOffset = new DateTimeOffset(universalTime);
			DateTimeOffset localAgainOffset = universalOffset.ToLocalTime();
			DateTime localAgainTime = localAgainOffset.LocalDateTime;
			Assert.AreEqual(localTime, localAgainTime, "local time equals local again time");
			Assert.AreEqual(localOffset, localAgainOffset, "local offset equals local again offset");
			Assert.IsTrue(localOffset.EqualsExact(localAgainOffset), "local offset exactly equals local again offset");
		}

		[TestMethod]
		public void TestSpecifyKind() {
			DateTime utc = new DateTime(2017, 05, 01, 12, 0, 0, DateTimeKind.Utc);
			DateTime utcRedundant = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
			Assert.AreEqual(utc, utcRedundant);
		}

		[TestMethod]
		public void TestCentralToUniversalToCentral() {
		}

		[TestMethod]
		public void TestTimeZoneAbbreviations() {
			string timeZoneId = "Eastern Standard Time";
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			Assert.IsNotNull(timeZoneInfo);
			Assert.AreEqual(timeZoneId, timeZoneInfo.Id);

			string cultureName = CultureInfo.CurrentCulture.Name;
			Assert.AreEqual("en-US", cultureName);

			TimeZoneValues abbreviations = TZNames.GetAbbreviationsForTimeZone(timeZoneId, cultureName);
			Assert.AreEqual("ET", abbreviations.Generic);
			Assert.AreEqual("EST", abbreviations.Standard);
			Assert.AreEqual("EDT", abbreviations.Daylight);

			TimeZoneValues names = TZNames.GetNamesForTimeZone(timeZoneId, cultureName);
			Assert.AreEqual("Eastern Time", names.Generic);
			Assert.AreEqual(timeZoneId, names.Standard);
			Assert.AreEqual("Eastern Daylight Time", names.Daylight);
		}
	}
}
