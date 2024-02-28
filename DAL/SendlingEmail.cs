using System;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL
{
	public class SendlingEmail
	{
		private readonly IEmailSender emailSender;

		public SendlingEmail(IEmailSender email)
		{
			emailSender = email ?? throw new ArgumentNullException(nameof(email));
		}

		public async Task<bool> SendEmail(string email, string subject, string message, Attachment attachment)
		{
			try
			{
				return await emailSender.SendEmailAsync(email, subject, message, attachment);
			}
			catch (Exception ex)
			{
				// Consider logging the exception or providing more meaningful information
				throw new Exception($"Error sending email: {ex.Message}", ex);
			}
		}
	}
}
