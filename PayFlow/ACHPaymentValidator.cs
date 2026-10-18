using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class ACHPaymentValidator : IPaymentValidator
    {
        public bool Validate(Payment payment)
        {
            if (payment is ACHPayment achPayment)
            {
                if (string.IsNullOrWhiteSpace(achPayment.TraceNumber))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public bool CanValidate(Payment payment)
        {
            return payment is ACHPayment;
        }
    }
}
