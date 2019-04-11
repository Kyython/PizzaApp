using BraintreeHttp;
using PayPal.Core;
using PayPal.v1.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaApp.Services.Utils
{
    public class PaymentSystem
    {
        public async Task<string> PayPalPaymentAsync(int orderPrice)
        {
            var environment = new SandboxEnvironment("AR_Hs07d93iQYwXhs9EQ6IDyzPfQEglNyWASQy0ge5LzeFgBa_KCcLefYAH1k-AhbiFxYM9R812rrTQT", "EHvkxwY2zbqtPEVtzs5lzsuLU1nBOP3mdndiM_iPnZt0A1gLuDdUjXW3GBrdXpaXpxGscjCZ37VMcCk-");
            var client = new PayPalHttpClient(environment);

            var payment = new Payment
            {
                Intent = "order",

                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = orderPrice.ToString(),
                            Currency = "USD"
                        }
                    }
                },
                RedirectUrls = new RedirectUrls()
                {

                    CancelUrl = "https://example.com/cancel",
                    ReturnUrl = "https://example.com/return"
                },

                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                return response.StatusCode.ToString();
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return statusCode.ToString();
            }
        }
    }
}
