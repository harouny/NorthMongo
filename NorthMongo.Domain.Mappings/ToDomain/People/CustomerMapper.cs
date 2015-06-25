using System.Linq;
using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    public class CustomerMapper : IMapToNew<EF.Customer, Customer>
    {
        public Customer Map(EF.Customer source)
        {
            if (source == null) return null;
            var customerDemgraphicsMapper = new CustomerDemographicMapper();
            return new Customer()
            {
                Region = source.Region,
                Address = source.Address,
                ContactName = source.ContactName,
                City = source.City,
                CompanyName = source.CompanyName,
                ContactTitle = source.ContactTitle,
                Country = source.Country,
                CustomerId = source.CustomerID,
                Phone = source.Phone,
                PostalCode = source.PostalCode,
                Fax = source.Fax,
                CustomerDemographics = source.CustomerDemographics.Select(obj => customerDemgraphicsMapper.Map(obj))
            };
        }
    }
}
