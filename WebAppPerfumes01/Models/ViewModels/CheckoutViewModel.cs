using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebAppPerfumes01.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        /*   public string Governorate { get; set; }
           public double ShippingPrice { get; set; }
           public List<string> ShippingOptions { get; set; }*/
     

        public string PaymentMethod { get; set; }

        public string Address { get; set; }

        public decimal Total { get; set; }
        public int TotalQuantity => CartItems?.Sum(i => i.Quantity) ?? 0;

        public List<DeliveryArea> DeliveryLocations { get; set; } // المواقع
    }
}
