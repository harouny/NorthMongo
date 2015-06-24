using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NorthMongo.Domain.Mappings.ToDomain.Categories;
using NorthMongo.Domain.Mappings.ToDomain.Products;
using NorthMongo.EF;
using Products = NorthMongo.Domain.Products;
using Categories = NorthMongo.Domain.Categories;

namespace NorthMongo.SQLToMongo
{
    class Program
    {
        private const string ProductsCollection = "Products";
        private const string CategoriesCollection = "Categories";
        
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var task = MigrateSqlToMongo();
            Console.WriteLine("Copying data from SQL to MongoDB");
            while (task.IsCompleted != true)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Task.WaitAll(task);
        }

        public static async Task MigrateSqlToMongo()
        {
            SetupConventions();

            var entities = new NorthwindEntities();
            var mongoClient = GetMongoClient();
            var mongoDatabase = GetMongoDatabase(mongoClient);
            
            //Cleanup before copy
            await mongoDatabase.DropCollectionAsync(ProductsCollection);
            await mongoDatabase.DropCollectionAsync(CategoriesCollection);

            //Copy Products
            var productsCollection = GetProductsCollection(mongoDatabase);
            var productMapper = new ProductMapper();            
            var products = (await entities
                                  .Products.ToListAsync()
                                  .ConfigureAwait(false))
                .Select(productEntity => productMapper.Map(productEntity));

            await productsCollection.InsertManyAsync(products)
                .ConfigureAwait(false);
            
            //Copy Categories
            var categoriesCollection = GetCategoriesCollection(mongoDatabase);
            var categoryMapper = new CategoryMapper();
            var categories = (await entities
                                 .Categories.ToListAsync()
                                 .ConfigureAwait(false))
               .Select(categoryEntity => categoryMapper.Map(categoryEntity));

            await categoriesCollection.InsertManyAsync(categories)
                .ConfigureAwait(false);
        }


        private static void SetupConventions()
        {
            ConventionRegistry.Register("camel case", 
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                }, t => true);
        }


        private static MongoClient GetMongoClient()
        {
            return new MongoClient(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);
        }

        private static IMongoDatabase GetMongoDatabase(MongoClient mongoClient)
        {
            return mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDbName"]);
        }

        private static IMongoCollection<Products.Product> GetProductsCollection(IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<Products.Product>(ProductsCollection);
        }

        private static IMongoCollection<Categories.Category> GetCategoriesCollection(IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<Categories.Category>(CategoriesCollection);
        }

    }
}
