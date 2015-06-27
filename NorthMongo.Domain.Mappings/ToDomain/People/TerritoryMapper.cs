using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    public class TerritoryMapper : IMapToNew<EF.Territory, Territory>
    {
        public Territory Map(EF.Territory source)
        {
            if (source == null) return null;
            return new Territory
            {
                RegionId = source.RegionID,
                TerritoryDescription = source.TerritoryDescription.Trim(),
                TerritoryId = source.TerritoryID,
                Region = new RegionMapper().Map(source.Region)
            };
        }
    }
}
