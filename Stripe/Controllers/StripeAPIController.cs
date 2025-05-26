using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe.Climate;
using Stripe.Models;
using Stripe.Options;

namespace Stripe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeAPIController : ControllerBase
    {
        private readonly ProductModel DefaultProduct = new ProductModel(1, "Wireless Earphone", 49.99m);

        private readonly StripeOptions _stripeOptions;


        public StripeAPIController(IOptionsSnapshot<StripeOptions> stripeOptions)
        {
            _stripeOptions = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
        }


        [HttpGet("SessionStatus")]
        public ActionResult SessionStatus([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            return Ok(new { status = session.Status, customer_email = session.CustomerDetails.Email,session = session });
        }

        [HttpPost]
        public async Task<IActionResult> BuyAsync([FromBody] BuyModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                if (model.ProductId != DefaultProduct.Id)
                {
                    return BadRequest("Product not found.");
                }

                var product = DefaultProduct;
                var origin = $"{Request.Scheme}://{Request.Host}";

                StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
                var stripeSessionService = new SessionService();

                var stripeCheckoutSession = await stripeSessionService.CreateAsync(
                    new SessionCreateOptions
                    {
                        UiMode = "embedded",
                        ReturnUrl = origin + "/confirmation.html?session_id={CHECKOUT_SESSION_ID}",
                        Mode = "payment",
                        ClientReferenceId = Guid.NewGuid().ToString(),
                        //CustomerEmail = model.Email,
                        LineItems = new()
                        {
                        new()
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "USD",
                                ProductData = new ()
                                {
                                    Name = product.Name
                                },
                                UnitAmountDecimal = (long)(product.Price * 100),
                            },
                            Quantity = 1,
                        }
                        }
                    });

                return Ok(new { clientSecret = stripeCheckoutSession.ClientSecret,message="Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        } 
    }
}
