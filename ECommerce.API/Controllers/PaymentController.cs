using ECommerce.Application.Features.Payments.Commands.UpdatePayment; // 👈 اتأكد من الـ Namespace بتاعك
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.V2.Core;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly string _stripeSecret;

        public PaymentsController(IConfiguration configuration)
        {
            
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

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    

                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    var orderId = int.Parse(session.Metadata["OrderId"]);
                    var transactionId = session.PaymentIntentId; 

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