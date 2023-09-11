using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum EOrderStatus
    {
        [Description("Mới")]
        New = 1,
        [Description("Chờ thanh toán")]
        Pending = 2,
        [Description("Đã thanh toán")]
        Paid = 3,
        [Description("Đang giao")]
        Shipping = 4,
        [Description("Hoàn thành")]
        Fulfilled = 5

    }
}
