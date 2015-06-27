using NorthMongo.EF;

namespace NorthMongo.Domain.Mappings.ToDomain.Products
{
    internal class SupplierMapper : IMapToNew<Supplier, Domain.Products.Supplier>
    {
        public Domain.Products.Supplier Map(Supplier source)
        {
            if (source == null) return null;
            return new Domain.Products.Supplier
            {
                CompanyName = source.CompanyName,
                SupplierId = source.SupplierID
            };
        }
    }
}
