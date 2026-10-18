using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public interface IPaymentValidator
    {
        bool CanValidate(Payment payment);
        bool Validate(Payment payment);
    }
}
