using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow
{
    public class PaymentService
    {
        private readonly List<IPaymentValidator> _validators;

        public event PaymentNotification? PaymentMatched;

        public PaymentService(List<IPaymentValidator> validators)
        {
            if (validators == null || validators.Count == 0)
            {
                throw new ArgumentException(
                    "At least one payment validator is required.");
            }
            _validators = validators;
        }

        public void Process(Payment payment)
        {
            bool isValid = true;

            bool hasApplicableValidator = false;

            bool hasGeneralValidator = _validators.Any(
                validator => validator is PaymentValidator
            );
            bool hasSpecificValidator = _validators.Any(validator =>
                (payment is CheckPayment && validator is CheckPaymentValidator) ||
                (payment is ACHPayment && validator is ACHPaymentValidator) ||
                (payment is WirePayment && validator is WirePaymentValidator)
            );

            if (!hasGeneralValidator)
            {
                Console.WriteLine("General payment validator is missing.");
                return;
            }
            if (!hasSpecificValidator)
            {
                Console.WriteLine(
                    $"No payment-type validator found for {payment.PaymentReference}.");
                return;
            }

            foreach (IPaymentValidator validator in _validators)
            {
                if (validator.CanValidate(payment))
                {
                    hasApplicableValidator = true;

                    bool result = validator.Validate(payment);

                    Console.WriteLine($"{validator.GetType().Name}: {result}");

                    if (!result)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            if (!isValid)
            {
                Console.WriteLine("Payment validation failed.");
                Console.WriteLine();
                return;
            }

            

            if (!hasApplicableValidator)
            {
                Console.WriteLine("No applicable validator found.");
                return;
            }

            



            Console.WriteLine("Payment validation successful.");

            Console.WriteLine($"Initial Status: {payment.Status}");
            try
            {
                payment.MarkAsProcessing();

                payment.ProcessPayment();

                payment.MarkAsMatched();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                if (payment.Status == PaymentStatus.Processing)
                {
                    payment.MarkAsFailed();
                }
                Console.WriteLine($"Final Status: {payment.Status}");

                return;
            }

            // Only runs after successful processing
            PaymentMatched?.Invoke(
                $"Payment {payment.PaymentReference} was successfully matched!");

            Console.WriteLine($"Final Status: {payment.Status}");
            Console.WriteLine();
        }
    }
}
