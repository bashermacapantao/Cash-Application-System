using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class Invoice
    {
        //An invoice is a formal business document sent by a seller to a buyer to request payment for goods or services provided.
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public InvoiceStatus Status { get; set; }
        

        public Invoice(int id, int invoiceNumber, int customerId, decimal amount, DateTime invoiceDate, InvoiceStatus status)
        {
            Id = id;
            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            Amount = amount;
            InvoiceDate = invoiceDate;
            Status = status;
        }

    }
}
