using NorthMongo.EF;

namespace NorthMongo.Domain.Mappings.ToDomain.Shippers
{
    public class ShipperMapper : IMapToNew<Shipper, Domain.Shippers.Shipper>
    {
        public Domain.Shippers.Shipper Map(Shipper source)
        {
            return new Domain.Shippers.Shipper()
            {
                CompanyName = source.CompanyName,
                Phone = source.Phone,
                ShipperId = source.ShipperID
            };
        }
    }
}
