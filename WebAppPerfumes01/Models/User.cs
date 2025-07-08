using System.ComponentModel.DataAnnotations;

namespace WebAppPerfumes01.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "User" or "Admin"
    }
}
