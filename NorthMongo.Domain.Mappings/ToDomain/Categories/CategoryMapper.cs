using NorthMongo.Domain.Categories;
namespace NorthMongo.Domain.Mappings.ToDomain.Categories
{
    public class CategoryMapper : IMapToNew<EF.Category, Category>
    {
        public Category Map(EF.Category source)
        {
            if (source == null) return null;
            var category = new Category
            {
                CategoryId = source.CategoryID,
                CategoryName = source.CategoryName,
                Description = source.Description,
                Picture = source.Picture
            };
            return category;
        }
    }
}
