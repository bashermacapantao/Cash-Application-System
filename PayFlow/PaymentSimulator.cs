using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class PaymentSimulator
    {
        public async Task SimulatePaymentAsync(Payment payment)
        {
            Console.WriteLine($"Starting payment: {payment.PaymentReference}");

            await Task.Delay(3000);

            Console.WriteLine($"Payment completed: {payment.PaymentReference}");
        }
    }
}
