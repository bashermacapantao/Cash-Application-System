using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class PaymentValidator : IPaymentValidator
    {
        public bool Validate(Payment payment)
        {
            if (string.IsNullOrWhiteSpace(payment.PaymentReference))
            {
                return false;
            }
            if (payment.CustomerId <= 0)
            {
                return false;
            }
            if (payment.Amount <= 0)
            {
                return false;
            }

            return true;
        }
        public bool CanValidate(Payment payment)
        {
            return true;
        }
    }
}
