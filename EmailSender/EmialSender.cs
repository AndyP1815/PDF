using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace EmailSender
{
	public class EmialSender : IEmailSender
	{
		public async Task<bool> SendEmailAsync(string email, string subject, string message, Attachment attachment)
		{
			try
			{
				var mail = "info@chaletcouvert.nl";
				var pw = "Chalet2015123!@";

				using (var client = new SmtpClient("send.one.com"))
				{
					client.Port = 587;
					client.EnableSsl = true;
					client.Credentials = new NetworkCredential(mail, pw);

					using (var mailMessage = new MailMessage(mail, email, subject, message))
					{
						mailMessage.Attachments.Add(attachment);
						await client.SendMailAsync(mailMessage);
						return true; // Success
					}
				}
			}
			catch (Exception ex)
			{
				return false; // Failure
			}
		}
	}
}
