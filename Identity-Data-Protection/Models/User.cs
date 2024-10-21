using System.ComponentModel.DataAnnotations;

namespace Identity_Data_Protection.Models
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
