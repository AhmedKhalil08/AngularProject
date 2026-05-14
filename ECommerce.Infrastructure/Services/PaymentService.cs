using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly PayPalHttpClient _paypalClient;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;

            // إعداد بيئة PayPal
            var paypalEnvironment = new SandboxEnvironment(
                _configuration["PayPalSettings:ClientId"],
                _configuration["PayPalSettings:Secret"]
            );
            _paypalClient = new PayPalHttpClient(paypalEnvironment);

            // إعداد مفتاح Stripe
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(decimal amount, ECommerce.Domain.Enums.PaymentMethod method, int orderId)
        {
            return method switch
            {
                ECommerce.Domain.Enums.PaymentMethod.CashOnDelivery => new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentUrl = null,
                    Message = "Order confirmed, payment will be made upon delivery."
                },
                ECommerce.Domain.Enums.PaymentMethod.CreditCard => await CreateStripeCheckoutSessionAsync(amount, orderId),
                ECommerce.Domain.Enums.PaymentMethod.PayPal => await CreatePayPalOrderAsync(amount, orderId),
                _ => new PaymentResultDto
                {
                    IsSuccess = false,
                    Message = "Payment method not supported."
                }
            };
        }

        private async Task<PaymentResultDto> CreateStripeCheckoutSessionAsync(decimal amount, int orderId)
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
                            // تحويل للقرش (Cents)
                            UnitAmount = (long)(amount * 100),
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order #{orderId} - Ecobazar",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/checkout/success",
                CancelUrl = "http://localhost:4200/checkout/failed",
                // 💡 الربط السحري مع الـ Webhook
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", orderId.ToString() }
                }
            };

            try
            {
                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentUrl = session.Url,
                    Message = "Please Complete Payment with Stripe."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResultDto { IsSuccess = false, Message = $"Stripe Error: {ex.Message}" };
            }
        }

        private async Task<PaymentResultDto> CreatePayPalOrderAsync(decimal amount, int orderId)
        {
            var domain = _configuration["AppUrl"] ?? "https://localhost:7123";

            // تحويل تقريبي للدولار (بما أن باي بال لا يدعم الجنيه حالياً)
            decimal amountInUsd = Math.Round(amount / 50m, 2);

            var orderRequest = new PayPalCheckoutSdk.Orders.OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        ReferenceId = orderId.ToString(),
                        // 💡 الربط السحري مع الـ PayPal Webhook
                        CustomId = orderId.ToString(),
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = amountInUsd.ToString("0.00")
                        },
                        Description = $"Ecobazar Order #{orderId}"
                    }
                },
                ApplicationContext = new PayPalCheckoutSdk.Orders.ApplicationContext
                {
                    ReturnUrl = $"{domain}/checkout/paypal-success?orderId={orderId}",
                    CancelUrl = $"{domain}/checkout/cancel",
                    UserAction = "PAY_NOW"
                }
            };

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(orderRequest);

            try
            {
                var response = await _paypalClient.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                var approveLink = result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href;

                return new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentUrl = approveLink,
                    Message = "Please approve the payment through PayPal."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResultDto { IsSuccess = false, Message = $"PayPal Error: {ex.Message}" };
            }
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