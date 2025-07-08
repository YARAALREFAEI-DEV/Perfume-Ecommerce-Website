using System.ComponentModel.DataAnnotations;

namespace WebAppPerfumes01.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }
        public bool IsActive { get; set; }

    }
}
