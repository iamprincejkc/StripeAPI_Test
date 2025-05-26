namespace Stripe.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ProductModel(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
