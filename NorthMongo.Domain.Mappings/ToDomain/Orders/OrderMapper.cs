using System.Linq;
using NorthMongo.Domain.Orders;

namespace NorthMongo.Domain.Mappings.ToDomain.Orders
{
    internal class OrderMapper : IMapToNew<EF.Order, Order>
    {
        public Order Map(EF.Order source)
        {
            if (source == null) return null;
            var orderDetailsMapper = new OrderDetailMapper();
            var customerMapper = new CustomerMapper();
            var employeeMapper = new EmployeeMapper();
            var shipperMapper = new ShipperMapper();
            return new Order
            {
                CustomerId = source.CustomerID,
                EmployeeId = source.EmployeeID,
                Freight = source.Freight,
                OrderDate = source.OrderDate,
                OrderId = source.OrderID,
                RequiredDate = source.RequiredDate,
                ShipAddress = source.ShipAddress,
                ShipCity = source.ShipCity,
                ShipCountry = source.ShipCountry,
                ShipName = source.ShipName,
                ShipPostalCode = source.ShipPostalCode,
                ShipRegion = source.ShipRegion,
                ShipVia = source.ShipVia,
                ShippedDate = source.ShippedDate,
                OrderDetails = source.Order_Details.Select(obj => orderDetailsMapper.Map(obj)),
                Customer = customerMapper.Map(source.Customer),
                Employee = employeeMapper.Map(source.Employee),
                Shipper = shipperMapper.Map(source.Shipper)
            };
        }
    }
}
