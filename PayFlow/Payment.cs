using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public abstract class Payment
    {
        public int Id { get; set; }
        public string PaymentReference { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public PaymentStatus Status { get; private set; }



        public Payment(int id, string paymentReference, int customerId, decimal amount, DateTime paymentDate, PaymentType paymentMethod)
        {
            Id = id;
            PaymentReference = paymentReference;
            CustomerId = customerId;

            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");
            }
            Amount = amount;

            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            Status = PaymentStatus.Received;

        }
        //Encapsulation means the Payment object controls how its data can be changed.
        /*Bussiness Rules.
        1. payment amount must be greater than 0.
        2. paymeny must recieved first before it was process and matched.*/
        public void MarkAsProcessing()
        {
            if (Status != PaymentStatus.Received)
            {
                throw new InvalidOperationException(
                    "Only received payments can be moved to processing.");
            }
            
            Status = PaymentStatus.Processing;
        }

        public void MarkAsMatched()
        {
            if (Status != PaymentStatus.Processing)
            {
                throw new InvalidOperationException(
                    "Only processing payments can be marked as matched.");
            }
            Status = PaymentStatus.Matched;

        }

        public void Cancel()
        {
            if (Status != PaymentStatus.Received && Status != PaymentStatus.Processing)
            {
                throw new InvalidOperationException(
                    "Only received or processing payments can be cancelled.");
            }
            Status = PaymentStatus.Cancelled;
        }

        //Poly
        public abstract void ProcessPayment();

        public void MarkAsFailed()
        {
            if (Status != PaymentStatus.Processing)
            {
                throw new InvalidOperationException(
                    "Only processing payments can be marked as failed.");
            }

            Status = PaymentStatus.Failed;
        }


        //A check payment means the customer pays using a physical or electronic check.
        //ACH = Automated Clearing House.
        //wire payment is also a bank transfer, but it's a different payment network/process from ACH.

    }
}
