using ECommerce.Application.DTOs;
using ECommerce.Application.Services;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Stripe;
using Stripe.Checkout;

namespace ECommerce.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly PayPalHttpClient _paypalClient;
        public PaymentService(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            var paypalEnvironment = new SandboxEnvironment(_configuration["PayPalSettings:ClientId"], _configuration["PayPalSettings:Secret"]);
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            _paypalClient = new PayPalHttpClient(paypalEnvironment);
        }
        public async Task<PaymentResultDto> ProcessPaymentAsync(decimal amount, ECommerce.Domain.Enums.PaymentMethod method, int orderId)
        {
            switch (method)
            {
                case ECommerce.Domain.Enums.PaymentMethod.CashOnDelivery:
                    return new PaymentResultDto
                    {
                        IsSuccess = true,
                        PaymentUrl = null,
                        Message = "Order confirmed, payment will be made upon delivery."
                    };

                case ECommerce.Domain.Enums.PaymentMethod.CreditCard:
                    // Call Stripe function to create a checkout session
                    return await CreateStripeCheckoutSessionAsync(amount);

                case ECommerce.Domain.Enums.PaymentMethod.PayPal:
                    return await CreatePayPalOrderAsync(amount, orderId);

                default:
                    return new PaymentResultDto
                    {
                        IsSuccess = false,
                        Message = "Payment method not supported."
                    };
            }

        }
        private async Task<PaymentResultDto> CreatePayPalOrderAsync(decimal amount, int orderId)
        {
            var domain = _configuration["AppUrl"] ?? "https://localhost:7123";

            // 💡 تحويل العملة: PayPal مابيدعمش الجنيه، فهنفترض إن الدولار بـ 50 جنيه مثلاً (طبعاً يفضل تجيب السعر من API حقيقي)
            decimal amountInUsd = Math.Round(amount / 50m, 2);

            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE", // لازم تقول لـ باي بال إن نيتك تسحب الفلوس
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        ReferenceId = orderId.ToString(), // بنربط طلب باي بال برقم الأوردر بتاعنا
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD", // 👈 دولار أمريكي
                            Value = amountInUsd.ToString("0.00") // باي بال بيحتاج الرقم كـ String بنقطة عشرية
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    // اللينك ده اللي الفرونت إند هيستقبل عليه العميل بعد ما يوافق
                    ReturnUrl = $"{domain}/checkout/paypal-success?orderId={orderId}",
                    CancelUrl = $"{domain}/checkout/cancel"
                }
            };

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(orderRequest);

            try
            {
                // إرسال الطلب لـ PayPal
                var response = await _paypalClient.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                // بنطلع اللينك اللي نوعه "approve" عشان نبعته للعميل
                var approveLink = result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href;

                return new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentUrl = approveLink, //  We'll redirect the customer to this link
                    Message = "please approve the payment through PayPal."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResultDto { IsSuccess = false, Message = ex.Message };
            }
        }
        private async Task<PaymentResultDto> CreateStripeCheckoutSessionAsync(decimal amount)
        {
            var domain = _configuration["AppUrl"] ?? "https://localhost:7123";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            // Stripe بيتعامل بالقروش (Cents)، فبنضرب في 100
                            UnitAmount = (long)(amount * 100),
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Ecommerce Order",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = domain + "/api/payments/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/api/payments/cancel",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new PaymentResultDto
            {
                IsSuccess = true,
                PaymentUrl = session.Url,
                Message = "please Complete Payment with Stripe."
            };
        }
        public async Task<bool> CapturePayPalPaymentAsync(string paypalOrderId)
        {
            var request = new OrdersCaptureRequest(paypalOrderId);
            request.RequestBody(new OrderActionRequest());

            try
            {
                var response = await _paypalClient.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                return result.Status == "COMPLETED";
            }
            catch
            {
                return false; 
            }
        }
    }

}
