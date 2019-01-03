using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using TimeZoneConverter;

namespace PilotFort.Classes
{
	public static class Extension {
		public const string DATE_TIME_FORMAT = "g";
		public const string DATE_FORMAT = "d";
		public const string TIME_FORMAT = "t";

		public static string ToAbsoluteUrl(this string virtualPath, string scheme = null) {
			HttpContext context = HttpContext.Current;
			string authority = Application.HostName;
			if (string.IsNullOrWhiteSpace(scheme)) {
				scheme = context == null ? "https" : context.Request.Url.Scheme;
			}
			if (string.IsNullOrWhiteSpace(authority)) {
				authority = context == null ? "www.densoconnect.com" : context.Request.Url.Authority;
			}
			string relativePath = context == null ? virtualPath.Substring(1, virtualPath.Length - 1) : VirtualPathUtility.ToAbsolute(virtualPath);
			return scheme + Uri.SchemeDelimiter + authority + relativePath;
		}

		public static byte[] ToBytes(this string hex) {
			int charCount = hex.Length;
			if (charCount % 2 == 1) {
				hex = "0" + hex;
				charCount++;
			}
			byte[] bytes = new byte[charCount / 2];
			for (int i = 0; i < charCount; i += 2) {
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}

		public static string ToDateDisplay(this DateTime date) {
			return date.ToString(DATE_FORMAT);
		}

		public static string ToDateDisplay(this DateTime? date) {
			return date == null ? string.Empty : ToDateDisplay(date.Value);
		}

		public static string ToDateTimeDisplay(this DateTime dateTime) {
			return dateTime.ToString(DATE_TIME_FORMAT);
		}

		public static string ToDateTimeDisplay(this DateTime? dateTime) {
			return dateTime == null ? string.Empty : ToDateTimeDisplay(dateTime.Value);
		}

		public static string ToHex(this byte[] bytes, bool lowerCase = false) {
			StringBuilder hex = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes) {
				hex.Append(b.ToString(lowerCase ? "x2" : "X2"));
			}
			return hex.ToString();
			// alternate
			//string hex = BitConverter.ToString(bytes);
			//return hex.Replace("-", string.Empty);
		}

		public static string ToString(this string[] strings, string[] separators) {
			if (strings == null) {
				throw new ArgumentNullException("strings");
			}
			if (separators == null) {
				throw new ArgumentNullException("separators");
			}
			if (strings.Length != separators.Length) {
				throw new ArgumentException("separators must have same number of elements as strings");
			}
			var result = new StringBuilder(128);
			for (int i = 0; i < strings.Length; i++) {
				if (!string.IsNullOrEmpty(strings[i])) {
					result.Append(separators[i]);
					result.Append(strings[i]);
				}
			}
			return result.ToString();
		}

		public static string ToTimeDisplay(this DateTime time) {
			return time.ToString(TIME_FORMAT);
		}

		public static string ToTimeDisplay(this DateTime? time) {
			return time == null ? string.Empty : ToTimeDisplay(time.Value);
		}

		public static TimeZoneInfo ToTimeZoneInfo(string windowsOrIanaTimeZoneId) {
			try {
				// Try a direct approach first
				return TimeZoneInfo.FindSystemTimeZoneById(windowsOrIanaTimeZoneId);
			} catch {
				// We have to convert to the opposite platform
				//bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
				//if (!isWindows) { timeZoneId = TZConvert.WindowsToIana(windowsOrIanaTimeZoneId); }
				var timeZoneId = TZConvert.IanaToWindows(windowsOrIanaTimeZoneId);
				return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			}
		}

		public static DateTime ToTimeZone(this DateTime utcDateTime, string timeZoneId) {
			TimeZoneInfo userTimeZone = ToTimeZoneInfo(timeZoneId);
			return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, userTimeZone);
		}

		public static DateTime ToUserTimeZone(this DateTime utcDateTime) {
			return ToTimeZone(utcDateTime, Security.UserTimeZoneId);
		}

		public static DateTime? ToUserTimeZone(this DateTime? utcDateTime) {
			return utcDateTime == null ? null : (DateTime?)ToUserTimeZone(utcDateTime.Value);
		}

		public static DateTime? ToUtcTime(this DateTime? userDateTime) {
			return userDateTime == null ? null : (DateTime?)ToUtcTime(userDateTime.Value);
		}

		public static DateTime ToUtcTime(this DateTime userDateTime) {
			return ToUtcTime(userDateTime, Security.UserTimeZoneId);
		}

		public static DateTime ToUtcTime(this DateTime userDateTime, string timeZoneId) {
			TimeZoneInfo userTimeZone = ToTimeZoneInfo(timeZoneId);
			return TimeZoneInfo.ConvertTimeToUtc(userDateTime, userTimeZone);
		}

		public static int ToUnixTimestamp(this DateTime utcDate) {
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = utcDate - origin;
			return Convert.ToInt32(Math.Floor(diff.TotalSeconds));
		}

		public static T? ToNullable<T>(this T t) where T : struct {
			return t.Equals(default(T)) ? null : (T?)t;
		}

		public static T ToValue<T>(this T? t) where T : struct {
			return t.HasValue ? t.Value : default(T);
		}

		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue)) {
			TValue val;
			return key != null && dict.TryGetValue(key, out val) ? val : defaultValue;
		}

		public static string ToPhoneNumber(this string s) {
			if (string.IsNullOrWhiteSpace(s)) {
				return string.Empty;
			}
			long n;
			if (!long.TryParse(s.Trim(), out n) || n < 10000) {
				return s;
			}
			if (n >= 10000000000) {
				return string.Format("+{0} ({1}) {2}-{3}", n / 10000000000, n % 10000000000 / 10000000, n % 10000000 / 10000, n % 10000);
			}
			if (n >= 10000000) {
				return string.Format("({0}) {1}-{2}", n / 10000000, n % 10000000 / 10000, n % 10000);
			}
			return string.Format("{0}-{1}", n / 10000, n % 10000);
		}

		public static string ToZipCode(this string s) {
			if (string.IsNullOrWhiteSpace(s)) {
				return string.Empty;
			}
			int n;
			if (!int.TryParse(s.Trim(), out n)) {
				return s;
			}
			if (n < 100000) {
				return n.ToString().PadLeft(5, '0');
			}
			return string.Format("{0}-{1}", (n / 10000).ToString().PadLeft(5, '0'), (n % 10000).ToString().PadLeft(4, '0'));
		}

		public static string ToQueryString(this NameValueCollection source) {
			return String.Join("&", source.AllKeys
				 .SelectMany(key => source.GetValues(key)
					  .Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))))
				 .ToArray());
		}

		public static string Truncate(this string s, int length) {
			if (s == null || s.Length <= length || (s = s.TrimEnd()).Length <= length) {
				return s;
			}
			return s.Substring(0, length);
		}

		public static T Deserialize<T>(this string json) {
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
				var settings = new DataContractJsonSerializerSettings() {
					DateTimeFormat = new DateTimeFormat("yyyy-mm-dd hh:MM:ss"),
					UseSimpleDictionaryFormat = true,
				};
				var serializer = new DataContractJsonSerializer(typeof(T), settings);
				return (T)serializer.ReadObject(stream);
			}
		}

		static readonly Random random = new Random();

		/// <summary>
		/// Fisher-Yates shuffle
		/// </summary>
		/// <param name="array">Array to shuffle in place</param>
		static void Shuffle(this int[] array) {
			int size = array.Length;
			for (int i = 0; i < size; i++) {
				int random = i + (int)(Extension.random.NextDouble() * (size - i));
				int temp = array[random];
				array[random] = array[i];
				array[i] = temp;
			}
		}

		public static string Shuffle(this string s) {
			if (s == null || s.Length < 2) {
				return s;
			}
			int size = s.Length;

			int[] sequence = new int[size];
			for (int i = 0; i < size; i++) {
				sequence[i] = i;
			}
			Shuffle(sequence);

			char[] output = new char[s.Length];
			for (int i = 0; i < size; i++) {
				output[sequence[i]] = s[i];
			}
			return new string(output);
		}
	}
}