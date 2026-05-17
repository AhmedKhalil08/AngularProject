using ECommerce.Application.Features.Payments.Commands.UpdatePayment; // 👈 اتأكد من الـ Namespace بتاعك
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly string _stripeSecret;
        private readonly IPaymentService _paymentService;
        public PaymentsController(IConfiguration configuration, IPaymentService paymentService)
        {

            _stripeSecret = configuration["StripeSettings:WebhookSecret"];
            _paymentService = paymentService;
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

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"🚨 Stripe Signature Error: {e.Message}");
                return BadRequest("Invalid Stripe Signature");
            }
            catch (Exception ex)
            {
                // السطرين دول هيطبعوا الإيرور بالتفصيل في شاشة الكونسول عندك
                Console.WriteLine($"🚨 الكود ضرب هنا: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
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
        [HttpGet("/api/auth/checkout/paypal-success")]
        public async Task<IActionResult> PayPalSuccess([FromQuery] string token, [FromQuery] string PayerID, [FromQuery] int orderId)
        {
            // 1. سحب الفلوس
            var isCaptured = await _paymentService.CapturePayPalPaymentAsync(token);

            if (isCaptured)
            {

                await Mediator.Send(new ConfirmPaymentCommand
                {
                    OrderId = orderId,
                    TransactionId = token
                });

                return Redirect("http://localhost:4200/checkout/success");
            }

            return Redirect("http://localhost:4200/checkout/failed");
        }
        [HttpGet("/api/auth/checkout/cancel")]
        public IActionResult PayPalCancel()
        {
            return Redirect("http://localhost:4200/checkout/failed");
        }
    }


}