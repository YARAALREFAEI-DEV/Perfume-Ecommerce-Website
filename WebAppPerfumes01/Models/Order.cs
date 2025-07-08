using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPerfumes01.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        // الربط مع المستخدم
        [ForeignKey("UserId")]
        public string UserId { get; set; } = String.Empty;
        //public User User { get; set; }  // علاقة Navigation مع جدول المستخدمين

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


        public DateTime OrderDate { get; set; } = DateTime.Now;
/*
        [Column("Status")]
        [Required]
        public OrderStatus Statu { get; set; }*/

       // public int DeliveryAreaId { get; set; }
        //public DeliveryArea DeliveryArea { get; set; }

        // حساب السعر النهائي
       // public decimal SubTotal => OrderDetails.Sum(item => item.Price);

       // public decimal DeliveryFee => DeliveryArea?.DeliveryFee ?? 0;

       // public decimal Total => SubTotal + DeliveryFee;

        public string FullName { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string PaymentMethod { get; set; }
        public int Quantity { get; set; }




     /*   public enum OrderStatus
        {
            [Display(Name = "بانتظار")]
            Pending,

            [Display(Name = "قيد المعالجة")]
            Processing,

            [Display(Name = "تم التوصيل")]
            Delivered
        }*/
    }
}
