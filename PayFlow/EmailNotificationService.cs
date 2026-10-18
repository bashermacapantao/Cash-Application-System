using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class EmailNotificationService : INotificationService
    {
        public void SendPaymentConfirmation(Payment payment)
        {
            Console.WriteLine(
            $"Payment {payment.PaymentReference} confirmed.");
        }
    }
}
