using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ScheduledJob
{
    public class ReminderCheckoutOrderDto
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string EmailContent { get; set; }

        public DateTimeOffset enqueueAt { get; set; }

    }
}
