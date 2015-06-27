namespace NorthMongo.Domain.Shippers
{
    public class Shipper : BaseEntity
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
