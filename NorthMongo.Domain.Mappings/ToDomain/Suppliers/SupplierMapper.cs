using NorthMongo.EF;

namespace NorthMongo.Domain.Mappings.ToDomain.Suppliers
{
    public class SupplierMapper : IMapToNew<Supplier, Domain.Suppliers.Supplier>
    {
        public Domain.Suppliers.Supplier Map(Supplier source)
        {
            return new Domain.Suppliers.Supplier()
            {
                City = source.City,
                Address = source.Address,
                CompanyName = source.CompanyName,
                ContactName = source.ContactName,
                ContactTitle = source.ContactTitle,
                Country = source.Country,
                Fax = source.Fax,
                HomePage = source.HomePage,
                Phone = source.Phone,
                PostalCode = source.PostalCode,
                Region = source.Region,
                SupplierId = source.SupplierID
            };
        }
    }
}
