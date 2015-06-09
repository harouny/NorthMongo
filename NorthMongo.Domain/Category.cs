using System.Collections.Generic;

namespace NorthMongo.Domain
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
    
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    
        public ICollection<Product> Products { get; set; }
    }
}
