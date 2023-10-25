using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class BackgroundJobSettings
    {
        public string HangfireUrl { get; set; }
        public string ApiGatewayUrl { get; set; }
        public string BasketUrl { get; set; }
        public string ScheduleJobUrl { get; set; }
    }
}
