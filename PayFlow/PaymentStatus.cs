using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public enum PaymentStatus
    {
        Received,
        Processing,
        Matched,
        Unmatched,
        Failed,
        Cancelled
    }
}
