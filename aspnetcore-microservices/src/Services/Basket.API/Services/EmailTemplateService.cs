using Hangfire;
using Shared.Configurations;

namespace Basket.API.Services
{
    public class EmailTemplateService
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _tmplFoder = Path.Combine(_baseDirectory, "EmailTemplates");

        protected readonly BackgroundJobSettings backgroundJobSettings;
        public EmailTemplateService(BackgroundJobSettings settings)
        {
            backgroundJobSettings = settings;
        }
        protected string ReadEmailTemplateContent(string templateEmailName, string format = "html")
        {
            var filePath = Path.Combine(_tmplFoder, templateEmailName + "." + format);
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs);

            var emailText = sr.ReadToEnd();
            sr.Close();

            return emailText;
        }
    }
}
