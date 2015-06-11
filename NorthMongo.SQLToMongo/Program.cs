using MongoDB.Driver;
using NorthMongo.Domain.Mappings.ToDomain;
using NorthMongo.EF;

namespace NorthMongo.SQLToMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var entities = new NorthwindEntities();
            var mongoClient = new MongoClient();
            var productMapper = new ProductMapper();
            foreach (var productEntity in entities.Products)
            {
                var product = productMapper.Map(productEntity);
                //TODO: save product to mongo
            }
        }
    }
}
