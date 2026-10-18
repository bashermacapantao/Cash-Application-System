using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class CheckPaymentValidator : IPaymentValidator
    {
        public bool Validate(Payment payment)
        {
            if (payment is CheckPayment checkPayment)
            {
                if (string.IsNullOrWhiteSpace(checkPayment.CheckNumber))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        public bool CanValidate(Payment payment)
        {
            return payment is CheckPayment;
        }
    }
}
