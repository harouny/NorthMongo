namespace NorthMongo.Domain.Orders
{
    public class Shipper : BaseEntity
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
    }
}
