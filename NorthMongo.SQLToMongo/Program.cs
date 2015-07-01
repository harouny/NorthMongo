using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NorthMongo.Domain.Mappings.ToDomain.Categories;
using NorthMongo.Domain.Mappings.ToDomain.Orders;
using NorthMongo.Domain.Mappings.ToDomain.People;
using NorthMongo.Domain.Mappings.ToDomain.Products;
using NorthMongo.Domain.Mappings.ToDomain.Shippers;
using NorthMongo.Domain.Mappings.ToDomain.Suppliers;
using NorthMongo.EF;
using Category = NorthMongo.Domain.Categories.Category;
using Customer = NorthMongo.Domain.People.Customer;
using Employee = NorthMongo.Domain.People.Employee;
using Order = NorthMongo.Domain.Orders.Order;
using Product = NorthMongo.Domain.Products.Product;
using Shipper = NorthMongo.Domain.Shippers.Shipper;
using Supplier = NorthMongo.Domain.Suppliers.Supplier;
using Territory = NorthMongo.Domain.People.Territory;

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
            var suppliersCollection = GetCollection<Supplier>(mongoDatabase, SuppliersCollectionName);
            var supplierMapper = new SupplierMapper();
            var suppliers = (await entities
                                .Suppliers.ToListAsync())
                .Select(supplierEntity => supplierMapper.Map(supplierEntity))
                .ToList();
            foreach (var supplier in suppliers)
            {
                await suppliersCollection
                    .InsertOneAsync(supplier);
            }


            //Copy Shippers
            var shippersCollection = GetCollection<Shipper>(mongoDatabase, ShippersCollectionName);
            var shipperMapper = new ShipperMapper();
            var shippers = (await entities
                                .Shippers.ToListAsync())
                .Select(shipperEntity => shipperMapper.Map(shipperEntity))
                .ToList();
            foreach (var shipper in shippers)
            {
                await shippersCollection
                    .InsertOneAsync(shipper);
            }



            //Copy Territories
            var territoryCollection = GetCollection<Territory>(mongoDatabase, TerritoriesCollection);
            var territoryMapper = new TerritoryMapper();
            var territories = (await entities
                                .Territories.ToListAsync())
                .Select(territoryEntity => territoryMapper.Map(territoryEntity))
                .ToList();
            foreach (var territory in territories)
            {
                await territoryCollection
                    .InsertOneAsync(territory);
            }


            //Copy Employees
            var employeeCollection = GetCollection<Employee>(mongoDatabase, EmployeesCollection);
            var employeeMapper = new EmployeeMapper();
            var employees = (await entities
                                .Employees.ToListAsync())
                .Select(employeeEntity => employeeMapper.Map(employeeEntity))
                .ToList();
            SyncEmployeesEmbededIds(employees, territories);
            foreach (var employee in employees)
            {
                await employeeCollection
                    .InsertOneAsync(employee);
            }


            //Copy Customers
            var customersCollection = GetCollection<Customer>(mongoDatabase, CustomersCollection);
            var customerMapper = new CustomerMapper();
            var customers = (await entities
                                .Customers.ToListAsync())
                .Select(customerEntity => customerMapper.Map(customerEntity))
                .ToList();
            foreach (var customer in customers)
            {
                await customersCollection
                    .InsertOneAsync(customer);
            }
            

            //Copy Categories
            var categoriesCollection = GetCollection<Category>(mongoDatabase, CategoriesCollectionName);
            var categoryMapper = new CategoryMapper();
            var categories = (await entities
                                 .Categories.ToListAsync())
               .Select(categoryEntity => categoryMapper.Map(categoryEntity))
               .ToList();
            foreach (var category in categories)
            {
                await categoriesCollection
                    .InsertOneAsync(category);
            }
            
            //Copy Products
            var productsCollection = GetCollection<Product>(mongoDatabase, ProductsCollectionName);
            var productMapper = new ProductMapper();
            var products = (await entities
                                  .Products.ToListAsync())
                .Select(productEntity => productMapper.Map(productEntity))
                .ToList();
            SyncProductsEmbededIds(products, suppliers, categories);
            await productsCollection.InsertManyAsync(products);


            //Copy Orders
            var ordersCollection = GetCollection<Order>(mongoDatabase, OrdersCollectionName);
            var orderMapper = new OrderMapper();
            var orders = (await entities
                                .Orders.ToListAsync())
                .Select(orderEntity => orderMapper.Map(orderEntity))
                .ToList();
            SyncOrdersEmbededIds(orders, shippers, employees, customers);
            await ordersCollection.InsertManyAsync(orders);

        }

        private static void SyncEmployeesEmbededIds(List<Employee> employees, List<Territory> allTerritoryDocuments)
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

        private static void SyncOrdersEmbededIds(List<Order> orders, List<Shipper> allShippersDocuments, List<Employee> allEmployeesDocuments, List<Customer> allCustomersDocuments)
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

        private static void SyncProductsEmbededIds(List<Product> products, List<Supplier> allSuppliersDocuments, List<Category> allCategoryDocuments)
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
