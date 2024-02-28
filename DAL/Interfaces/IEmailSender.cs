using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IEmailSender
	{
		Task<bool> SendEmailAsync(string email,string subject,string message, Attachment attachment);
	}
}
