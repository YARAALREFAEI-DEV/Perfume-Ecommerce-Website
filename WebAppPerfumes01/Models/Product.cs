using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPerfumes01.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }  
        [NotMapped]
        public IFormFile? PhotoFile { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; } // نسبة الخصم (مثلاً 0.15 = خصم 15%)
        public string Category { get; set; }
      
        // السعر بعد الخصم
        public decimal FinalPrice => Discount.HasValue ? Price * (1 - Discount.Value) : Price;
    }

}
