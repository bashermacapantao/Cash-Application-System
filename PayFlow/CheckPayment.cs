using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class CheckPayment : Payment
    {
        public string CheckNumber { get; private set; }

        public CheckPayment(
            int id, 
            string paymentReference, 
            int customerId, 
            decimal amount, 
            DateTime paymentDate, 
            PaymentType paymentMethod,
            string checkNumber)
            : base(id, 
                  paymentReference, 
                  customerId, 
                  amount, 
                  paymentDate,
                  paymentMethod)
        {
            CheckNumber = checkNumber;
        }
        public override void ProcessPayment()
        {
            Console.WriteLine("Processing check payment..");
        }
    }
}
