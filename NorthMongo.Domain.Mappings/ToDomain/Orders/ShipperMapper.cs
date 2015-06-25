using NorthMongo.Domain.Orders;

namespace NorthMongo.Domain.Mappings.ToDomain.Orders
{
    internal class ShipperMapper : IMapToNew<EF.Shipper, Shipper>
    {
        public Shipper Map(EF.Shipper source)
        {
            if (source == null) return null;
            return new Shipper
            {
                ShipperId = source.ShipperID,
                CompanyName = source.CompanyName
            };
        }
    }
}
