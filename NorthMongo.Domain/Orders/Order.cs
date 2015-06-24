using System;
using System.Collections.Generic;

namespace NorthMongo.Domain.Orders
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public Shipper Shipper { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
