namespace NorthMongo.Domain.Orders
{
    public class Customer : BaseEntity
    {
        public string CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
    }
}
