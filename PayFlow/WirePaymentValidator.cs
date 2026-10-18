using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class WirePaymentValidator : IPaymentValidator
    {
        public bool Validate(Payment payment)
        {
            if (payment is WirePayment wirePayment)
            {
                if (string.IsNullOrWhiteSpace(wirePayment.WireReference))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public bool CanValidate(Payment payment)
        {
            return payment is WirePayment;
        }
    }
}
