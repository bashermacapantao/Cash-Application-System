// See https://aka.ms/new-console-template for more information
using PayFlow;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;

// 1. Create customer
static Customer CreateCustomer()
{
    Customer customer = new Customer(
    1,
    "Test",
    "Test@gmail.com",
    new List<Invoice>(),
    new List<Payment>()
    );
    return customer;
}

// 2. Add invoices
static List<Invoice> CreateInvoices()
{
    List<Invoice> invoices = new List<Invoice>();

    Invoice invoice1 = new Invoice(
        1, 123456789, 
        1, 69.9m,
        DateTime.Now, 
        InvoiceStatus.Paid
    );

    Invoice invoice2 = new Invoice(
        2, 987654321, 
        1, 1000m,
        DateTime.Now, 
        InvoiceStatus.Cancelled
    );

    invoices.Add(invoice1);
    invoices.Add(invoice2);

    return invoices;
}


// 3. Add payments
static List<Payment> CreatePayments()
{
    List<Payment> payments = new();

    Payment payment1 = new CheckPayment(
        1, "PAY001", 1, 1000m,
        DateTime.Now, PaymentType.Check, "CHK001"
    );

    Payment payment2 = new ACHPayment(
        2, "PAY002", 1, 9m,
        DateTime.Now, PaymentType.ACH, "ACH002"
    );

    Payment payment3 = new WirePayment(
        3, "PAY003", 1, 1000m,
        DateTime.Now, PaymentType.Wire, "WIRE003"
    );

    payments.Add(payment1);
    payments.Add(payment2);
    payments.Add(payment3);

    return payments;
}

//adding Invoices and Payments to Customer
Customer customer = CreateCustomer();

List<Invoice> invoices = CreateInvoices();

customer.Invoices.AddRange(invoices);

List<Payment> payments = CreatePayments();

customer.Payments.AddRange(payments);


// 4. Register validators
static List<IPaymentValidator> CreateValidators()
{
    List<IPaymentValidator> validators = new();

    validators.Add(new PaymentValidator());
    validators.Add(new CheckPaymentValidator());
    validators.Add(new ACHPaymentValidator());
    validators.Add(new WirePaymentValidator());

    return validators;
}

List<IPaymentValidator> validators = CreateValidators();

// 5. Create payment service
PaymentService paymentService = new PaymentService(validators);

// 6. Subscribe to events
PaymentNotification notification = ShowNotification;
static void ShowNotification(string message)
{
    Console.WriteLine($"NOTIFICATION: {message}");
}
paymentService.PaymentMatched += ShowNotification;
paymentService.PaymentMatched += (message) =>
{
    Console.WriteLine($"AUDIT LOG: {message}");
    
};

// 7. Display customer and invoices
static void DisplayCustomer(Customer customer)
{
    Console.WriteLine("=== Customer ===");
    Console.WriteLine($"Id: {customer.Id}");
    Console.WriteLine($"Name: {customer.Name}");
    Console.WriteLine($"Email: {customer.Email}");
}

DisplayCustomer(customer);
Console.WriteLine();


static void DisplayInvoices(List<Invoice> invoices)
{
    Console.WriteLine("=== Invoices ===");

    foreach (Invoice invoice in invoices)
    {
        Console.WriteLine($"Invoice Number: {invoice.InvoiceNumber}");
        Console.WriteLine($"Amount: {invoice.Amount}");
        Console.WriteLine($"Status: {invoice.Status}");
        Console.WriteLine();
    }
}

DisplayInvoices(customer.Invoices);
Console.WriteLine();


static void DisplayPayments(List<Payment> payments)
{
    Console.WriteLine("=== Payments ===");

    foreach (Payment payment in payments)
    {
        Console.WriteLine($"Reference: {payment.PaymentReference}");
        Console.WriteLine($"Amount: {payment.Amount}");
        Console.WriteLine($"Status: {payment.Status}");
        Console.WriteLine();
    }
}

DisplayPayments(customer.Payments);
Console.WriteLine();


// 8. Process payments
static void ProcessPayments(
    List<Payment> payments,
    PaymentService paymentService)
{
    foreach (Payment payment in payments)
    {
        paymentService.Process(payment);
    }
}

// 9. Display updated payment information
ProcessPayments(customer.Payments, paymentService);
Console.WriteLine();

// 10. Generate LINQ reports
static void GeneratePaymentReport(List<Payment> payments)
{
    var matchedPayments = payments
        .Where(payment => payment.Status == PaymentStatus.Matched)
        .ToList();

    decimal totalAmount = matchedPayments
        .Sum(payment => payment.Amount);

    Console.WriteLine("=== Payment Report ===");
    Console.WriteLine($"Matched Payments: {matchedPayments.Count}");
    Console.WriteLine($"Total Matched Amount: {totalAmount}");

    var groupedPayments = matchedPayments
    .GroupBy(payment => payment.PaymentMethod);

    foreach (var group in groupedPayments)
    {
        decimal methodTotal = group
            .Sum(payment => payment.Amount);

        Console.WriteLine($"Payment Method: {group.Key}");
        Console.WriteLine($"Total Amount: {methodTotal}");
    }
}

GeneratePaymentReport(customer.Payments);

Console.WriteLine();
Console.WriteLine("=== Invalid Status Transition Test ===");

Payment testPayment = new CheckPayment(
    4,
    "PAY004",
    1,
    500m,
    DateTime.Now,
    PaymentType.Check,
    "CHECK004"
);

try
{
    testPayment.MarkAsMatched();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine($"Final Status: {testPayment.Status}");
Console.WriteLine();


Payment testPayment2 = new CheckPayment(
    5, "PAY005", 1, 500m,
    DateTime.Now, PaymentType.Check, "CHECK005"
);

Console.WriteLine($"Initial Status: {testPayment2.Status}");

testPayment2.MarkAsProcessing();

testPayment2.MarkAsMatched();

Console.WriteLine($"Final Status: {testPayment2.Status}");
Console.WriteLine();


Console.WriteLine("=== Duplicate Processing Test ===");

try
{
    testPayment2.MarkAsProcessing();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine($"Final Status: {testPayment2.Status}");
Console.WriteLine();

PaymentSimulator simulator = new PaymentSimulator();

List<Task> tasks = new();

foreach (Payment payment in customer.Payments)
{
    tasks.Add(simulator.SimulatePaymentAsync(payment));
}

await Task.WhenAll(tasks);

int successfulCount = tasks.Count(
    task => task.Status == TaskStatus.RanToCompletion);

int failedCount = tasks.Count(
    task => task.Status == TaskStatus.Faulted);

Console.WriteLine("=== Async Payment Summary ===");
Console.WriteLine($"Successful: {successfulCount}");
Console.WriteLine($"Failed: {failedCount}");
Console.WriteLine();

/*Console.WriteLine("=== Empty Validator Test ===");

try
{
    List<IPaymentValidator> emptyValidators = new();

    PaymentService testService = new PaymentService(emptyValidators);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}*/

List<IPaymentValidator> testValidators = new()
{
    new PaymentValidator(),
    new CheckPaymentValidator(),
    new WirePaymentValidator()
};

PaymentService testService = new PaymentService(testValidators);

ACHPayment testACH = new ACHPayment(
    5,
    "PAY005",
    1,
    500m,
    DateTime.Now,
    PaymentType.ACH,
    "ACH005"
);

testService.Process(testACH);

Console.WriteLine($"Final Status: {testACH.Status}");
Console.WriteLine();

List<IPaymentValidator> testValidatorss = new()
{
    new CheckPaymentValidator(),
    new ACHPaymentValidator(),
    new WirePaymentValidator()
};

PaymentService testServices = new PaymentService(testValidatorss);
CheckPayment testCheck = new CheckPayment(
    6,
    "PAY006",
    1,
    500m,
    DateTime.Now,
    PaymentType.Check,
    "CHK006"
);

testServices.Process(testCheck);

Console.WriteLine($"Final Status: {testCheck.Status}");

/*Method Responsibility
CreateCustomer()	        Creates a customer
CreateInvoices()	        Creates invoice objects
CreatePayments()	        Creates payment objects
CreateValidators()	        Registers validators
DisplayCustomer()	        Displays customer information
DisplayInvoices()	        Displays invoices
DisplayPayments()	        Displays payments
ProcessPayments()	        Calls PaymentService for each payment
GeneratePaymentReport()	    Generates LINQ reports
*/

/*
Payment payment1 = new CheckPayment(
    1,
    "PAY001",
    1,
    1000,
    DateTime.Now,
    PaymentType.Check,
    "CHECK001"
    
);
Payment payment2 = new ACHPayment(
    2,
    "PAY002",
    1,
    9,
    DateTime.Now,
    PaymentType.ACH,
    "ACH002"
);

Payment payment3 = new WirePayment(
    3,
    "PAY003",
    1,
    1000m,
    DateTime.Now,
    PaymentType.Wire,
    "WIRE003"
);


validators.Add(new PaymentValidator());
validators.Add(new CheckPaymentValidator());
validators.Add(new ACHPaymentValidator());
validators.Add(new WirePaymentValidator());

customer.Invoices.Add(invoice1);
customer.Invoices.Add(invoice2);
customer.Payments.Add(payment1);
customer.Payments.Add(payment2);
customer.Payments.Add(payment3);
*/
/*foreach (Invoice invoice in customer.Invoices)
{
    Console.WriteLine($"Invoice ID: {invoice.Id}");
    Console.WriteLine($"Invoice Number: {invoice.InvoiceNumber}");
    Console.WriteLine($"Amount: {invoice.Amount}");
    Console.WriteLine($"Invoice Date: {invoice.InvoiceDate}");
    Console.WriteLine($"Status: {invoice.Status}");
    Console.WriteLine($" ");
}

Console.WriteLine("\n=== Payments ===");
foreach (Payment payment in customer.Payments)
{
    Console.WriteLine($"Payment ID: {payment.Id}");
    Console.WriteLine($"Payment Reference: {payment.PaymentReference}");
    Console.WriteLine($"Amount: {payment.Amount}");
    Console.WriteLine($"Payment Date: {payment.PaymentDate}");
    Console.WriteLine($"Payment Method: {payment.PaymentMethod}");
    Console.WriteLine($"Status: {payment.Status}");
    paymentService.Process(payment);
    Console.WriteLine();
}*/


//CheckPaymentValidator validate = new CheckPaymentValidator();

//bool result = validate.Validate(payment2);

//Console.WriteLine($"result: {result}");

//List<Payment> matchedPayments = customer.Payments
//    .Where(payment => payment.Status == PaymentStatus.Matched)
//    .ToList();
//Console.WriteLine("\n=== Matched Payments ===");

//foreach (Payment payment in matchedPayments)
//{
//    Console.WriteLine(
//        $"Reference: {payment.PaymentReference}, Status: {payment.Status}");
//}

//decimal totalMatchedAmount = matchedPayments
//    .Sum(payment => payment.Amount);

//Console.WriteLine($"Total Matched Amount: {totalMatchedAmount}");

//bool hasFailedPayments = customer.Payments
//    .Any(payment => payment.Status == PaymentStatus.Failed);

//Console.WriteLine($"Has Failed Payments: {hasFailedPayments}");

//Payment? foundPayment = customer.Payments
//    .FirstOrDefault(payment =>
//        payment.PaymentReference == "PAY002");

//if (foundPayment != null)
//{
//    Console.WriteLine($"Payment Found: {foundPayment.PaymentReference}");
//    Console.WriteLine($"Amount: {foundPayment.Amount}");
//    Console.WriteLine($"Status: {foundPayment.Status}");
//}
//else
//{
//    Console.WriteLine("Payment not found.");
//}

//List<string> paymentReferences = customer.Payments
//    .Select(payment => payment.PaymentReference)
//    .ToList();

//foreach (string reference in paymentReferences)
//{
//    Console.WriteLine(reference);
//}

//List<string> matchedReferences = customer.Payments
//    .Where(payment => payment.Status == PaymentStatus.Matched)
//    .Select(payment => payment.PaymentReference)
//    .ToList();

//foreach (string reference in matchedReferences)
//{
//    Console.WriteLine(reference);
//}
//List<Payment> sortedPayments = customer.Payments
//    .OrderBy(payment => payment.Amount)
//    .ToList();

//foreach (Payment payment in sortedPayments)
//{
//    Console.WriteLine(
//        $"{payment.PaymentReference}: {payment.Amount}");
//}
//int matchedCount = customer.Payments
//    .Count(payment => payment.Status == PaymentStatus.Matched);

//Console.WriteLine($"Total Matched Payments: {matchedCount}");

//var groupedPayments = customer.Payments
//    .GroupBy(payment => payment.PaymentMethod);

//foreach (var group in groupedPayments)
//{
//    Console.WriteLine($"Payment Method: {group.Key}");
//    Console.WriteLine($"Total Payments: {group.Count()}");
//}
/*
var groupedPayments = customer.Payments
    .GroupBy(payment => payment.PaymentMethod);

foreach (var group in groupedPayments)
{
    decimal totalAmount = group.Sum(payment => payment.Amount);

    Console.WriteLine($"Payment Method: {group.Key}");
    Console.WriteLine($"Total Amount: {totalAmount}");
}

var report = customer.Payments
    .Where(payment => payment.Status == PaymentStatus.Matched)
    .GroupBy(payment => payment.PaymentMethod);

foreach (var group in report)
{
    decimal total = group.Sum(payment => payment.Amount);

    Console.WriteLine($"Method: {group.Key}");
    Console.WriteLine($"Total: {total}");
}

Console.WriteLine("Day 7 - Delegates");
static void ShowNotification(string message)
{
    Console.WriteLine($"NOTIFICATION: {message}");
}
PaymentNotification notification = ShowNotification;

notification("Payment PAY002 was matched!");

PaymentSimulator simulator = new PaymentSimulator();
Stopwatch stopwatch = Stopwatch.StartNew();

//await simulator.SimulatePaymentAsync(payment1);
//await simulator.SimulatePaymentAsync(payment2);
//await simulator.SimulatePaymentAsync(payment3);

Task task1 = simulator.SimulatePaymentAsync(payment1);
Task task2 = simulator.SimulatePaymentAsync(payment2);
Task task3 = simulator.SimulatePaymentAsync(payment3);

try
{
    await Task.WhenAll(task1, task2, task3);
    stopwatch.Stop();


    Console.WriteLine(
        $"Total execution time: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
    Console.WriteLine("All simulated payments completed!");
}
catch(Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
Console.WriteLine($"Check Payment: {task1.Status}");
Console.WriteLine($"ACH Payment: {task2.Status}");
Console.WriteLine($"Wire Payment: {task3.Status}");
if (task2.Status == TaskStatus.Faulted)
{
    Console.WriteLine("ACH payment needs investigation.");
}
List<Task> tasks = new List<Task>();
foreach (Payment payment in customer.Payments)
{
    tasks.Add(simulator.SimulatePaymentAsync(payment));
}
try
{
    await Task.WhenAll(tasks);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

List<(Payment Payment, Task Task)> paymentTasks = new();

foreach (Payment payment in customer.Payments)
{
    Task task = simulator.SimulatePaymentAsync(payment);
    paymentTasks.Add((payment, task));
}
try
{
    await Task.WhenAll(paymentTasks.Select(item => item.Task));
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
foreach (var item in paymentTasks)
{
    Console.WriteLine(
        $"Payment: {item.Payment.PaymentReference}");

    Console.WriteLine(
        $"Task Status: {item.Task.Status}");
}

int successfulCount = paymentTasks
    .Count(item => item.Task.Status == TaskStatus.RanToCompletion);

int failedCount = paymentTasks
    .Count(item => item.Task.Status == TaskStatus.Faulted);

Console.WriteLine("=== Async Payment Summary ===");
Console.WriteLine($"Successful: {successfulCount}");
Console.WriteLine($"Failed: {failedCount}");*/