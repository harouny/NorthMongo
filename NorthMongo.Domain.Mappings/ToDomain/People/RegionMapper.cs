using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    public class RegionMapper : IMapToNew<EF.Region, Region>
    {
        public Region Map(EF.Region source)
        {
            if (source == null) return null;
            return new Region()
            {
                RegionDescription = source.RegionDescription.Trim(),
                RegionId = source.RegionID
            };
        }
    }
}
