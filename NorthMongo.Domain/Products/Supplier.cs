namespace NorthMongo.Domain.Products
{
    public class Supplier : BaseEntity
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
    }
}
