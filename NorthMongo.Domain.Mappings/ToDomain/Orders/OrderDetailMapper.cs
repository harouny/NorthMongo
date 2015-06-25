using NorthMongo.Domain.Orders;

namespace NorthMongo.Domain.Mappings.ToDomain.Orders
{
    internal class OrderDetailMapper : IMapToNew<EF.Order_Detail, OrderDetail>
    {
        public OrderDetail Map(EF.Order_Detail source)
        {
            if (source == null) return null;
            return new OrderDetail
            {
                OrderId = source.OrderID,
                ProductId = source.ProductID,
                UnitPrice = source.UnitPrice,
                Quantity = source.Quantity,
                Discount = source.Discount
            };
        }
    }
}
