using System.Collections.Generic;

namespace NorthMongo.Domain
{
    public class Customer
    {
        //public Customer()
        //{
        //    Orders = new HashSet<Order>();
        //    CustomerDemographics = new HashSet<CustomerDemographic>();
        //}
    
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    
        //public ICollection<Order> Orders { get; set; }
        //public ICollection<CustomerDemographic> CustomerDemographics { get; set; }
    }
}
