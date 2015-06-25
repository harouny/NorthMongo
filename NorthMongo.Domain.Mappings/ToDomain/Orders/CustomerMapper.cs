using NorthMongo.Domain.Orders;

namespace NorthMongo.Domain.Mappings.ToDomain.Orders
{
    internal class CustomerMapper : IMapToNew<EF.Customer, Customer>
    {
        public Customer Map(EF.Customer source)
        {
            if (source == null) return null;
            return new Customer
            {
                CustomerId = source.CustomerID,
                ContactName = source.ContactName,
                ContactTitle = source.ContactTitle
            };
        }
    }
}
