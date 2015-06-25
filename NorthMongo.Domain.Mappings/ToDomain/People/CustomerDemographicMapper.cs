using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    public class CustomerDemographicMapper : IMapToNew<EF.CustomerDemographic, CustomerDemographic>
    {
        public CustomerDemographic Map(EF.CustomerDemographic source)
        {
            throw new System.NotImplementedException();
        }
    }
}
