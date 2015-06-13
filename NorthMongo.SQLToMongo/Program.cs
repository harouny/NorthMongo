using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using NorthMongo.Domain.Mappings.ToDomain;
using NorthMongo.EF;

namespace NorthMongo.SQLToMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = MigrateSqlToMongo();
            Task.WaitAll(task);
        }

        public static async Task MigrateSqlToMongo()
        {
            var entities = new NorthwindEntities();
            var mongoClient = GetMongoClient();
            var mongoDatabase = GetMongoDatabase(mongoClient);
            await mongoDatabase.DropCollectionAsync("Products");
            var productsCollection = GetProductsCollection(mongoDatabase);
            var productMapper = new ProductMapper();
            
            var products = (await entities
                                  .Products.ToListAsync()
                                  .ConfigureAwait(false))
                .Select(productEntity => productMapper.Map(productEntity));

            await productsCollection.InsertManyAsync(products)
                .ConfigureAwait(false);
        }


        public static MongoClient GetMongoClient()
        {
            return new MongoClient("mongodb://localhost");
        }

        public static IMongoDatabase GetMongoDatabase(MongoClient mongoClient)
        {
            return mongoClient.GetDatabase("Northwind");
        }

        public static IMongoCollection<Domain.Product> GetProductsCollection(IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<Domain.Product>("Products");
        }

    }
}
