using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using PilotFort.Models.Data;

namespace PilotFort.Classes
{
	public enum Role
	{
		INQUIRY_ONLY,
		EMPLOYEE_PURCHASE,
		CUSTOMER,
		SALESMAN,
		CUSTOMER_ADMINISTRATOR,
		ADMINISTRATOR,
	}

	public static class Security
	{
		const string SESSION_KEY_USER = "PILOTFORT_USER";
		const string SESSION_KEY_IMPERSONATING = "IMPERSONATING_USER";

		public static Guid UserId {
			get { return User.Id; }
		}

		public static Guid UserAccountId {
			get { return User.AccountId; }
		}

		public static string UserName {
			get { return Impersonating ?? IdentityName; }
		}

		public static string IdentityName {
			get { return HttpContext.Current == null ? string.Empty : HttpContext.Current.User.Identity.Name; }
		}

		// called from Views\Shared\_Layout.cshtml
		public static string UserNameDisplay {
			get { return IsImpersonating ? IdentityName + " as " + User.FirstName : User.FirstName; }
		}

		public static string UserEmail {
			get { return User.Email; }
		}

		public static string UserTimeZoneId {
			get { return User.TimeZone; }
		}

		public static string Impersonating {
			get {
				HttpContext context = HttpContext.Current;
				if (context == null) {
					return null;
				}
				HttpSessionState session = context.Session;
				object value = session[SESSION_KEY_IMPERSONATING];
				return value == null ? null : value.ToString();
			}
			set { HttpContext.Current.Session[SESSION_KEY_IMPERSONATING] = value; }
		}

		public static bool IsImpersonating {
			get { return Impersonating != null; }
		}

		public static bool IsUserSiteAdmin { get { return User.IsSiteAdmin; } }
		public static bool IsUserAccountAdmin { get { return User.IsAccountAdmin; } }

		/// <returns>Whether the user account identified by userName is accessible to the current user</returns>
		public static bool IsAccessible(Guid appUserId) {
			if (IsUserSiteAdmin) {
				return true;
			}
			if (!IsUserAccountAdmin) {
				return appUserId == UserId;
			}
			// account admins can only administer users associated to accounts the admin is on
			List<Account> accessibleAccounts = Data.AccountData.Default.GetUserAccounts(UserId);
			List<Account> userAccounts = Data.AccountData.Default.GetUserAccounts(appUserId);
			foreach (Account accessibleAccount in accessibleAccounts) {
				foreach (Account userAccount in userAccounts) {
					if (accessibleAccount.Id == userAccount.Id) {
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsRole(string roleName, Role role) {
			return string.Equals(roleName, role.ToString());
		}

		public static bool IsRoleAny(string roleName, params Role[] roles) {
			foreach (Role role in roles) {
				if (IsRole(roleName, role)) {
					return true;
				}
			}
			return false;
		}

		public static User UserClone() {
			return (User)User.Clone();
		}

		private static User User {
			get {
				User user = (User)HttpContext.Current.Session[SESSION_KEY_USER];
				string userName = UserName;
				if (user == null || !string.Equals(userName, user.UserName)) {
					user = Data.AccountData.Default.GetUser(userName);
					HttpContext.Current.Session[SESSION_KEY_USER] = user;
				}
				return user ?? User.NOBODY;
			}
		}

		public static void ClearUser() {
			HttpContext.Current.Session.Remove(SESSION_KEY_USER);
		}

		public static bool CheckPassword(string input, byte[] hash, byte hashType) {
			byte[] inputHash = Hash(input, hashType);
			bool match = inputHash.SequenceEqual(hash);
			if (!match) {
				// introduce a time delay to mitigate brute force attack
				System.Threading.Thread.Sleep(100);
			}
			return match;
		}

		/// <summary>
		/// Encrypt fingerprint for Authorize.net gateway (SIM = Server Integration Method)
		/// </summary>
		public static string Fingerprint(string input, string key) {
			byte[] keyBytes = Encoding.ASCII.GetBytes(key);
			byte[] payload = Encoding.ASCII.GetBytes(input);
			var md5 = new HMACMD5(keyBytes);
			byte[] hash = md5.ComputeHash(payload);
			return hash.ToHex();
		}

		/// <summary>
		/// Return MD5 hash of input string for verifying MD5 hash in Authorize.net response
		/// </summary>
		public static string HashMD5(string input) {
			byte[] payload = Encoding.ASCII.GetBytes(input);
			byte[] hash = MD5.Create().ComputeHash(payload);
			return hash.ToHex();
		}

		public const byte CurrentHashType = 2;

		public static byte[] Hash(string input, byte hashType) {
			byte[] hash;
			if (input == null) {
				return null;
			}
			if (hashType < 1 || hashType > CurrentHashType) {
				hashType = CurrentHashType;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			// legacy hashing algorithm from old website
			using (SHA1Managed sha1 = new SHA1Managed()) {
				hash = sha1.ComputeHash(bytes);
			}
			// PBKDF2 hashing algorithm using legacy hash as salt
			if (hashType == 2) {
				// this constructor defaults to 1000 iterations
				using (var pbkdf = new Rfc2898DeriveBytes(input, hash)) {
					hash = pbkdf.GetBytes(64);
				}
			}
			return hash;
		}

		// leave out lowercase L and uppercase i to avoid confusion
		public const string AllowedPasswordChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
		public static readonly IList<int> PasswordCharTypeBounds = new List<int>(new int[] { 0, 25, 50, 60, 66 }).AsReadOnly();

		public static string NewPassword(int length = 12) {
			if (length < 8) {
				throw new ApplicationException("minimum password length is 8");
			}

			var random = new RNGCryptoServiceProvider();
			char[] chars = new char[length];
			byte[] index = new byte[1];

			// first make sure we get at least one character of each type (lower, upper, number, special)
			IList<int> bounds = PasswordCharTypeBounds;
			int boundStarts = bounds.Count - 1;
			for (int i = 0; i < boundStarts; i++) {
				random.GetBytes(index);
				chars[i] = AllowedPasswordChars[bounds[i] + (index[0] % (bounds[i + 1] - bounds[i]))];
			}

			// fill remainder of length with any random characters
			for (int i = boundStarts; i < length; i++) {
				random.GetBytes(index);
				chars[i] = AllowedPasswordChars[index[0] % AllowedPasswordChars.Length];
			}

			var unshuffled = new string(chars);
			return unshuffled.Shuffle();
		}

		public static bool ValidatePassword(string newPassword) {
			if (newPassword == null) {
				return false;
			}

			var hasNumber = new Regex(@"[0-9]+");
			var hasLowerChar = new Regex(@"[a-z]+");
			var hasUpperChar = new Regex(@"[A-Z]+");
			var hasMinimum8Chars = new Regex(@".{8,}");

			return hasNumber.IsMatch(newPassword)
				&& hasLowerChar.IsMatch(newPassword)
				&& hasUpperChar.IsMatch(newPassword)
				&& hasMinimum8Chars.IsMatch(newPassword);
		}
	}

	public class AccountAdminAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			return base.AuthorizeCore(httpContext) && Security.IsUserAccountAdmin;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			filterContext.Result = new RedirectToRouteResult(
				new RouteValueDictionary(new {
					area = string.Empty,
					controller = "Home",
					action = "Index"
				}));
		}
	}

	public class SiteAdminAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			return base.AuthorizeCore(httpContext) && Security.IsUserSiteAdmin;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			filterContext.Result = new RedirectToRouteResult(
				new RouteValueDictionary(new {
					area = string.Empty,
					controller = "Home",
					action = "Index"
				}));
		}
	}
}