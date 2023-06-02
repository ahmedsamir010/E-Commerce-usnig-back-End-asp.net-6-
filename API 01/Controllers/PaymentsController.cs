using API_01.Controllers;
using API_01.Dtos;
using API_01.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.IO;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregrate;
using Talabat.Core.Services;

namespace Talabat.Controllers
{
 
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _webHookSecret = "whsec_9ab10c9fe46b48b7de853b30c090a66aa5b79af12ac190531e00d75bf67e31cd";

        public PaymentsController(IPaymentService paymentService , ILogger<PaymentsController> logger   )
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]  
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            try
            {
                var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
                if (basket == null)
                {
                    return NotFound(new ApiResponse(404, "Basket not found"));
                }

                return Ok(basket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating/updating payment intent: {ex.Message}"));
            }
        }

        [AllowAnonymous] // Allow anonymous access for Stripe webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webHookSecret);

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

                Order order; 
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment Failed ", paymentIntent.Id);
                        break;
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment Succeded ", paymentIntent.Id);
                        // Handle payment success event
                        break;
                    // Add additional event handling as needed
                    default:
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
    }
}
