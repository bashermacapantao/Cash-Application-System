using PayFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    //A customer can have multiple invoice and multiple payments.
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Invoice> Invoices { get; set; }

        public List<Payment> Payments { get; set; }

        public Customer(int id, string name, string email, List<Invoice> invoices, List<Payment> payments)
        {
            Id = id;
            Name = name;
            Email = email;
            Invoices = invoices;
            Payments = payments;
        }
    }
}
/*
Day 2 is complete

You've learned and applied:

Encapsulation — Status and Amount use private set
Validation — amount must be greater than zero
Business rules — payment transitions are controlled
Methods with behavior — MarkAsProcessing(), MarkAsMatched(), Cancel()
Exceptions — invalid operations throw InvalidOperationException
State transitions — Received → Processing → Matched
Constructor control — every new payment starts as Received
*/