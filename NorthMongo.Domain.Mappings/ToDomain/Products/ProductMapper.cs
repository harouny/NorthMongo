using NorthMongo.Domain.Products;
namespace NorthMongo.Domain.Mappings.ToDomain.Products
{
    public class ProductMapper : IMapToNew<EF.Product, Product>
    {
        public Product Map(EF.Product source)
        {
            if (source == null) return null;
            var categoryMapper = new CategoryMapper();
            var product = new Product
            {
                ProductId = source.ProductID,
                CategoryId = source.CategoryID,
                Discontinued = source.Discontinued,
                ProductName = source.ProductName,
                QuantityPerUnit = source.QuantityPerUnit,
                ReorderLevel = source.ReorderLevel,
                SupplierId = source.SupplierID,
                UnitPrice = source.UnitPrice,
                UnitsInStock = source.UnitsInStock,
                UnitsOnOrder = source.UnitsOnOrder,
                Category = categoryMapper.Map(source.Category)
            };
            return product;
        }
    }
}
