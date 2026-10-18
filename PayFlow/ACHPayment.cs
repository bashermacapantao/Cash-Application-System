using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class ACHPayment : Payment
    {
        public string TraceNumber { get; private set; }

        public ACHPayment(
           int id,
           string paymentReference,
           int customerId,
           decimal amount,
           DateTime paymentDate,
           PaymentType paymentMethod,
           string traceNumber)
           : base(id,
                 paymentReference,
                 customerId,
                 amount,
                 paymentDate,
                 paymentMethod)
        {
            TraceNumber = traceNumber;
        }
        public override void ProcessPayment()
        {
            Console.WriteLine("Processing ACH payment..");
        }
    }
}
