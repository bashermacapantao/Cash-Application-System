using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class WirePayment : Payment
    {
        public string WireReference { get; private set; }
        public WirePayment(int id, string paymentReference, int customerId, decimal amount, DateTime paymentDate, PaymentType paymentMethod, string wireReference) : base(id, paymentReference, customerId, amount, paymentDate, paymentMethod)
        {
            WireReference = wireReference;
        }
        public override void ProcessPayment()
        {
            Console.WriteLine("Processing WIRE payment..");
        }
    }
}
