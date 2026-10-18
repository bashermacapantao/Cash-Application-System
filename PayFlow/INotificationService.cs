using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public interface INotificationService
    {
        void SendPaymentConfirmation(Payment payment);
    }
}
