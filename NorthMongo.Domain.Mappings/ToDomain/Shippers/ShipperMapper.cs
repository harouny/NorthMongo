﻿using NorthMongo.EF;

namespace NorthMongo.Domain.Mappings.ToDomain.Shippers
{
    public class ShipperMapper : IMapToNew<Shipper, Domain.Shippers.Shipper>
    {
        public Domain.Shippers.Shipper Map(Shipper source)
        {
            if (source == null) return null;
            return new Domain.Shippers.Shipper()
            {
                CompanyName = source.CompanyName,
                Phone = source.Phone,
                ShipperId = source.ShipperID
            };
        }
    }
}
