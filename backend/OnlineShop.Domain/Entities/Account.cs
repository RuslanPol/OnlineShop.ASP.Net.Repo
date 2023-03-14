using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities
{
    public record Account : IEntity
    {
        public Guid Id { get; init; }

        [Required] public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required] [MinLength(6)] public string PasswordHash { get; set; }

        public Account(Guid id, string name, string email, string password)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = password ?? throw new ArgumentNullException(nameof(password));
        }
#pragma warning disable CS8618
        private Account()
        {
        }
#pragma warning restore CS8618
    }
}