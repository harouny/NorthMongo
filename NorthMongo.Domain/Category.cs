using System.Collections.Generic;

namespace NorthMongo.Domain
{
    public class Category
    {
        //public Category()
        //{
        //    Products = new List<Product>();
        //}
    
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        //TODO: binary Picture shouldn't be copied to each product
        //public byte[] Picture { get; set; }
    
        //public IEnumerable<Product> Products { get; set; }
    }
}
