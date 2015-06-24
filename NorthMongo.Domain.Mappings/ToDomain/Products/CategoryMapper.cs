using NorthMongo.Domain.Products;

namespace NorthMongo.Domain.Mappings.ToDomain.Products
{
    internal class CategoryMapper : IMapToNew<EF.Category, Category>
    {
        public Category Map(EF.Category source)
        {
            return new Category
            {
                CategoryId = source.CategoryID,
                CategoryName = source.CategoryName,
            };
        }
    }
}
