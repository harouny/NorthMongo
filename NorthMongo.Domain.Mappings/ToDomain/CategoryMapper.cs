namespace NorthMongo.Domain.Mappings.ToDomain
{
    public class CategoryMapper : IMapToNew<EF.Category, Category>
    {
        public Category Map(EF.Category source)
        {
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
