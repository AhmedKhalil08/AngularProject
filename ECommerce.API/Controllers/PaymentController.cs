using ECommerce.Application.Features.Payments.Commands.UpdatePayment; // 👈 اتأكد من الـ Namespace بتاعك
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly string _stripeSecret;

        // 1. حقن الـ IConfiguration لقراءة الإعدادات بأمان
        public PaymentsController(IConfiguration configuration)
        {
            // لازم تتأكد إنك ضايف السطر ده في ملف appsettings.json:
            // "Stripe": { "WebhookSecret": "whsec_..." }
            _stripeSecret = configuration["Stripe:WebhookSecret"];
        }

        // ==========================================
        // 1. Stripe Webhook
        // ==========================================
        [HttpPost("stripe-webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSecret
                );

                // 2. معالجة نجاح الدفع
                if (stripeEvent.Type == Stripe.EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    var orderId = int.Parse(paymentIntent.Metadata["OrderId"]);
                    var transactionId = paymentIntent.Id;

                    // 3. مناداة الـ Handler بالاسم الصحيح اللي اتفقنا عليه
                    await Mediator.Send(new ConfirmPaymentCommand
                    {
                        OrderId = orderId,
                        TransactionId = transactionId
                    });
                }
                // (اختياري) ممكن تضيف معالجة لـ Events.PaymentIntentPaymentFailed لو حابب تسجل إن الدفع فشل

                return Ok();
            }
            catch (StripeException)
            {
                // الـ Exception ده بيحصل لو التوقيع (Signature) مش متطابق، يعني الريكويست مش من Stripe
                return BadRequest("Invalid Stripe Signature");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ==========================================
        // 2. PayPal Webhook
        // ==========================================
        [HttpPost("paypal-webhook")]
        public async Task<IActionResult> PayPalWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var paypalEvent = System.Text.Json.JsonDocument.Parse(json);
                var eventType = paypalEvent.RootElement.GetProperty("event_type").GetString();

                if (eventType == "PAYMENT.CAPTURE.COMPLETED")
                {
                    var resource = paypalEvent.RootElement.GetProperty("resource");

                    var customId = resource.GetProperty("custom_id").GetString();
                    var transactionId = resource.GetProperty("id").GetString();

                    if (int.TryParse(customId, out int orderId))
                    {
                        // مناداة نفس الـ Handler
                        await Mediator.Send(new ConfirmPaymentCommand
                        {
                            OrderId = orderId,
                            TransactionId = transactionId
                        });
                    }
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}