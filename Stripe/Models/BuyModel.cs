using System.ComponentModel.DataAnnotations;

namespace Stripe.Models
{
    public class BuyModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
