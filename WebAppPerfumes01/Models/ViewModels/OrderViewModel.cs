using System.ComponentModel.DataAnnotations;

namespace WebAppPerfumes01.Models.ViewModels
{
    public class OrderViewModel
    {


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }  
        public string Email { get; set; } 

         [Required]
            [Phone]
            public string Phone { get; set; }       
        
        public int Quantity { get; set; }


        public string PaymentMethod { get; set; }


        //public string Address { get; set; }


   
        public decimal TotalPrice { get; set; }



        public string Location { get; set; }


        //public int TotalQuantity => CartItems?.Sum(i => i.Quantity) ?? 0;
        public List<CartItem> CartItems { get; set; }

        public List<DeliveryArea> DeliveryLocations { get; set; } // المواقع
       // public decimal GrandTotal { get; set; }
        //public decimal Subtotal { get; set; }
            public decimal Total { get; set; }

   


    }
}
