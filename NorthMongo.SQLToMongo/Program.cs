using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NorthMongo.Domain.Mappings.ToDomain.Categories;
using NorthMongo.Domain.Mappings.ToDomain.Orders;
using NorthMongo.Domain.Mappings.ToDomain.People;
using NorthMongo.Domain.Mappings.ToDomain.Products;
using NorthMongo.Domain.Mappings.ToDomain.Shippers;
using NorthMongo.Domain.Mappings.ToDomain.Suppliers;
using NorthMongo.EF;
using Products = NorthMongo.Domain.Products;
using Categories = NorthMongo.Domain.Categories;
using Shippers = NorthMongo.Domain.Shippers;
using Suppliers = NorthMongo.Domain.Suppliers;
using Orders = NorthMongo.Domain.Orders;
using People = NorthMongo.Domain.People;

namespace NorthMongo.SQLToMongo
{
    class Program
    {
        private const string ProductsCollectionName = "Products";
        private const string CategoriesCollectionName = "Categories";
        private const string ShippersCollectionName = "Shippers";
        private const string SuppliersCollectionName = "Suppliers";
        private const string OrdersCollectionName = "Orders";
        private const string TerritoriesCollection = "Territories";
        private const string EmployeesCollection = "Employees";
        private const string CustomersCollection = "Customers";
        
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
            var emptyFilter = new BsonDocument();

            
            //Cleanup before copy
            await mongoDatabase.DropCollectionAsync(ProductsCollectionName);
            await mongoDatabase.DropCollectionAsync(CategoriesCollectionName);
            await mongoDatabase.DropCollectionAsync(SuppliersCollectionName);
            await mongoDatabase.DropCollectionAsync(ShippersCollectionName);
            await mongoDatabase.DropCollectionAsync(OrdersCollectionName);
            await mongoDatabase.DropCollectionAsync(TerritoriesCollection);
            await mongoDatabase.DropCollectionAsync(EmployeesCollection);
            await mongoDatabase.DropCollectionAsync(CustomersCollection);

            //Copy Suppliers
            var suppliersCollection = GetCollection<Suppliers.Supplier>(mongoDatabase, SuppliersCollectionName);
            var supplierMapper = new SupplierMapper();
            var suppliers = (await entities
                                .Suppliers.ToListAsync()
                                .ConfigureAwait(false))
                .Select(supplierEntity => supplierMapper.Map(supplierEntity));
            await suppliersCollection.InsertManyAsync(suppliers);
            var allSuppliersDocuments = (await suppliersCollection
                           .Find(emptyFilter)
                           .ToListAsync()
                           .ConfigureAwait(false));


            //Copy Shippers
            var shippersCollection = GetCollection<Shippers.Shipper>(mongoDatabase, ShippersCollectionName);
            var shipperMapper = new ShipperMapper();
            var shippers = (await entities
                                .Shippers.ToListAsync()
                                .ConfigureAwait(false))
                .Select(shipperEntity => shipperMapper.Map(shipperEntity));
            await shippersCollection.InsertManyAsync(shippers);
            var allShippersDocuments = (await shippersCollection
                            .Find(emptyFilter)
                            .ToListAsync()
                            .ConfigureAwait(false));



            //Copy Territories
            var territoryCollection = GetCollection<People.Territory>(mongoDatabase, TerritoriesCollection);
            var territoryMapper = new TerritoryMapper();
            var territories = (await entities
                                .Territories.ToListAsync()
                                .ConfigureAwait(false))
                .Select(territoryEntity => territoryMapper.Map(territoryEntity));
            await territoryCollection.InsertManyAsync(territories);
            var allTerritoryDocuments = (await territoryCollection
                                        .Find(emptyFilter)
                                        .ToListAsync()
                                        .ConfigureAwait(false));


            //Copy Employees
            var employeeCollection = GetCollection<People.Employee>(mongoDatabase, EmployeesCollection);
            var employeeMapper = new EmployeeMapper();
            var employees = (await entities
                                .Employees.ToListAsync()
                                .ConfigureAwait(false))
                .Select(employeeEntity => employeeMapper.Map(employeeEntity))
                .ToList();
            SyncEmployeesEmbededIds(employees, allTerritoryDocuments);
            await employeeCollection.InsertManyAsync(employees);
            var allEmployeesDocuments = (await employeeCollection
                                        .Find(emptyFilter)
                                        .ToListAsync()
                                        .ConfigureAwait(false));


            //Copy Customers
            var customersCollection = GetCollection<People.Customer>(mongoDatabase, CustomersCollection);
            var customerMapper = new CustomerMapper();
            var customers = (await entities
                                .Customers.ToListAsync()
                                .ConfigureAwait(false))
                .Select(customerEntity => customerMapper.Map(customerEntity));
            await customersCollection.InsertManyAsync(customers);
            var allCustomersDocuments = (await customersCollection
                            .Find(emptyFilter)
                            .ToListAsync()
                            .ConfigureAwait(false));
            

            //Copy Categories
            var categoriesCollection = GetCollection<Categories.Category>(mongoDatabase, CategoriesCollectionName);
            var categoryMapper = new CategoryMapper();
            var categories = (await entities
                                 .Categories.ToListAsync()
                                 .ConfigureAwait(false))
               .Select(categoryEntity => categoryMapper.Map(categoryEntity));

            await categoriesCollection.InsertManyAsync(categories)
                .ConfigureAwait(false);
            var allCategoryDocuments = (await categoriesCollection
                           .Find(emptyFilter)
                           .ToListAsync()
                           .ConfigureAwait(false));
           
            
            //Copy Products
            var productsCollection = GetCollection<Products.Product>(mongoDatabase, ProductsCollectionName);
            var productMapper = new ProductMapper();
            var products = (await entities
                                  .Products.ToListAsync()
                                  .ConfigureAwait(false))
                .Select(productEntity => productMapper.Map(productEntity))
                .ToList();
            SyncProductsEmbededIds(products, allSuppliersDocuments, allCategoryDocuments);
            await productsCollection.InsertManyAsync(products)
                .ConfigureAwait(false);


            //Copy Orders
            var ordersCollection = GetCollection<Orders.Order>(mongoDatabase, OrdersCollectionName);
            var orderMapper = new OrderMapper();
            var orders = (await entities
                                .Orders.ToListAsync()
                                .ConfigureAwait(false))
                .Select(orderEntity => orderMapper.Map(orderEntity))
                .ToList();
            SyncOrdersEmbededIds(orders, allShippersDocuments, allEmployeesDocuments, allCustomersDocuments);
            await ordersCollection.InsertManyAsync(orders);

        }

        private static void SyncEmployeesEmbededIds(List<People.Employee> employees, List<People.Territory> allTerritoryDocuments)
        {
            foreach (var employee in employees)
            {
                foreach (var territory in employee.Territories)
                {
                    territory.Id = allTerritoryDocuments
                        .Single(obj => obj.TerritoryId == territory.TerritoryId).Id;
                }
            }
        }

        private static void SyncOrdersEmbededIds(List<Orders.Order> orders, List<Shippers.Shipper> allShippersDocuments, List<People.Employee> allEmployeesDocuments, List<People.Customer> allCustomersDocuments)
        {
            foreach (var order in orders)
            {
                order.Customer.Id = allCustomersDocuments
                    .Single(obj => obj.CustomerId == order.CustomerId).Id;
                if (order.EmployeeId.HasValue)
                {
                    order.Employee.Id = allEmployeesDocuments
                        .Single(obj => obj.EmployeeId == order.EmployeeId).Id;
                }
                if (order.ShipVia.HasValue)
                {
                    order.Shipper.Id = allShippersDocuments
                        .Single(obj => obj.ShipperId == order.ShipVia).Id;
                }
            }
        }

        private static void SyncProductsEmbededIds(List<Products.Product> products, List<Suppliers.Supplier> allSuppliersDocuments, List<Categories.Category> allCategoryDocuments)
        {
            foreach (var product in products)
            {
                if (product.CategoryId.HasValue)
                {
                    product.Category.Id = allCategoryDocuments
                        .Single(obj => obj.CategoryId == product.CategoryId).Id;
                }

                if (product.SupplierId.HasValue)
                {
                    product.Supplier.Id = allSuppliersDocuments
                        .Single(obj => obj.SupplierId == product.SupplierId).Id;
                }
            }
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

        private static IMongoCollection<T> GetCollection<T>(IMongoDatabase mongoDatabase, string collectionName)
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }

    }
}
