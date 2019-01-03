using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using PilotFort.Data;
using PilotFort.Models.Data;

namespace PilotFort.Classes
{
	public static class Emails
	{
		const string EMAIL_DISABLED_MESSAGE = "Email not enabled";

		public static string DefaultFrom {
			get { return ConfigurationManager.AppSettings["FromEmailAddress"]; }
		}

		public static string OverrideTo {
			get { return ConfigurationManager.AppSettings["ToEmailAddressOverride"]; }
		}

		public static bool IsEmailEnabled {
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsEmailEnabled"]); }
		}

		public static void SendEmail(Email email) {
			MailMessage message = new MailMessage();
			if (string.IsNullOrWhiteSpace(email.From)) {
				email.From = DefaultFrom;
			}
			if (string.IsNullOrWhiteSpace(email.To)) {
				email.To = OverrideTo;
				if (string.IsNullOrWhiteSpace(email.To)) {
					AdminData.Default.LogError("Attempted to send an email with no recipient");
					return;
				}
			}
			message.From = new MailAddress(email.From);
			message.To.Add(email.To);
			message.Subject = email.Subject;
			message.Body = email.Message;
			message.IsBodyHtml = true;

			if (email.Attachment != null) {
				message.Attachments.Add(email.Attachment);
			}
			SendEmail(message, email);
		}

		public static Email SendEmail(MailMessage message, Email log = null) {
			string userName = Security.IdentityName;
			string toAddressOverride = OverrideTo;
			bool toAddressOverridden = !string.IsNullOrWhiteSpace(toAddressOverride);
			Exception error = null;
			if (!IsEmailEnabled) {
				error = new Exception(EMAIL_DISABLED_MESSAGE);
			} else {
				if (toAddressOverridden) {
					message.To.Clear();
					message.To.Add(toAddressOverride);
				}
				for (int retry = 0; retry < 2; retry++) {
					error = null;
					try {
						// Rely on web.config for mail settings
						SmtpClient smtpClient = new SmtpClient();
						smtpClient.Send(message);
						break;
					} catch (Exception x) {
						error = x;
						// Wait before trying again
						System.Threading.Thread.Sleep(500);
					}
				}
			}

			if (log == null) {
				log = new Email();
				log.From = message.From.ToString();
				log.To = message.To.ToString();
				log.Subject = message.Subject;
				log.Message = message.Body;
			} else if (toAddressOverridden) {
				log.To = toAddressOverride;
			}
			log.UserName = userName;
			log.Error = error;

			// Log email, whether success or not
			AdminData.Default.LogEmail(log);
			if (error != null && !string.Equals(EMAIL_DISABLED_MESSAGE, error.Message)) {
				AdminData.Default.LogError(error, Security.IdentityName);
			}
			return log;
		}

		/// <summary>Depends on the OrderNumber being present on the Cart</summary>
//		public static Email SendOrderConfirmation(Cart cart, User user) {
//			if (cart == null) {
//				throw new ArgumentException("Cart not found for order number " + cart.OrderNumber);
//			}
//			if (user == null) {
//				throw new ArgumentException("Cart is missing User ID");
//			}
//			if (cart.OrderNumber <= 0) {
//				throw new ArgumentException("Cart is missing OrderNumber");
//			}
//			Order order = OrdersData.Default.GetOrder(cart.OrderNumber.Value,
//				user.CurrentCustomerNumber ?? 0, user.UserId, user.IsDensoAdministrator || user.IsDensoSalesRep, user.IsEmployee);
//			if (order == null) {
//				AdminData.Default.LogError("Cart has invalid order number " + cart.OrderNumber, Security.IdentityName);
//				return null;
//			}
//			order.BillingAddress = AccountData.Default.GetBillingAddress(order.CustomerNumber);

//			var mail = new MailMessage();
//			mail.From = new MailAddress(DefaultFrom);
//			mail.To.Add(new MailAddress(user.Email));
//			string applicationName = Application.Name;
//			mail.Subject = "Your " + applicationName + " Order Confirmation :: " + order.OrderNumber;

//			byte[] orderConfirmationPDF = Export.OrderConfirmation(order.OrderNumber, user);
//			var stream = new MemoryStream(orderConfirmationPDF);
//			string attachmentName = string.Format("{0}_OrderConfirmation_{1}.pdf", applicationName, order.OrderNumber);
//			var attachment = new Attachment(stream, attachmentName, MediaTypeNames.Application.Pdf);
//			mail.Attachments.Add(attachment);

//			string orderUrl = ("~/Orders/Index/" + cart.OrderNumber.Value).ToAbsoluteUrl();

//			mail.IsBodyHtml = true;
//			mail.Body = String.Format(@"<html>
//<body style='padding: 15px; font-family: OpenSans,Helvetica,Arial,sans-serif; font-size: 14px;'>"
////	<img src=""cid:inlineLogo"" alt='DENSO Logo' style='float: left; margin-right: 24px; margin-bottom: 20px;' />
//// <div style='padding-top: 20px;'><span style='font-size: 32px; white-space: nowrap;'><i>Order Confirmation</i></span></div>
//+ "<div style='clear: left;'>"
////<hr style='height: 4px; background-color: #eb181e; border: none; margin: 20px 0;' />
//+ @"<p>Hello {0} {1},</p>
//	<p>Thank you for placing your order with the {2} site. You will receive additional email(s) as your order is processed</p>
//	<p>Confirmation ID: {3}</p>
//	<p>Customer: {4} - {5}</p>
//	<p>Purchase Order #: {6}</p>
//	<p>Total: {7}</p>
//	<p>&nbsp;</p>
//	<p><a href=""{8}"">Click here to view your order online</a></p>
//	<p>PLEASE DO NOT RESPOND TO THIS MESSAGE</p>
//	<p>Thank you,</p>
//	<p>DENSO Sales</p>
//</div>
//</body>
//</html>", HttpUtility.HtmlEncode(user.FirstName), HttpUtility.HtmlEncode(user.LastName),
//			HttpUtility.HtmlEncode(applicationName), order.OrderNumber, order.CustomerNumber,
//			HttpUtility.HtmlEncode(order.CustomerName), HttpUtility.HtmlEncode(order.CustomerPurchaseOrder),
//			order.TotalAmount, orderUrl);

//			return SendEmail(mail);
//		}
	}
}