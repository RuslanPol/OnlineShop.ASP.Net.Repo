using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.Requests
{
    public class LogInRequest
    {
        [Required] public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required] [MinLength(6)] public string PasswordHash { get; set; }
    }
}