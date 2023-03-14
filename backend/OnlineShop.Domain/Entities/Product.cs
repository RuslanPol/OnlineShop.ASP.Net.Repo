namespace OnlineShop.Domain.Entities
{
    public record Product : IEntity
    {
        public Guid Id { get; init; }

        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        private Product()
        {
        }

        public Product(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}