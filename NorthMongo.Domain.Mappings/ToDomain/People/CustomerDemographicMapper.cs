using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    internal class CustomerDemographicMapper : IMapToNew<EF.CustomerDemographic, CustomerDemographic>
    {
        public CustomerDemographic Map(EF.CustomerDemographic source)
        {
            if (source == null) return null;
            return new CustomerDemographic()
            {
                CustomerDesc = source.CustomerDesc,
                CustomerTypeId = source.CustomerTypeID
            };
        }
    }
}
