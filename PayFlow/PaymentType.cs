using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public enum PaymentType
    {
        Check,
        ACH,
        Wire
    }
}
//A check payment means the customer pays using a physical or electronic check.
//ACH = Automated Clearing House.
//wire payment is also a bank transfer, but it's a different payment network/process from ACH.

/*Check Payment
    → needs CheckNumber

ACH Payment
    → needs BankAccount / TraceNumber

Wire Payment
    → needs WireReference*/