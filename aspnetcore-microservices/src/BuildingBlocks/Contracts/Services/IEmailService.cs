using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services
{
    public interface IEmailService<in T> where T : class
    {
        Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken());
        void SendEmail(MailRequest request);
    }
}
