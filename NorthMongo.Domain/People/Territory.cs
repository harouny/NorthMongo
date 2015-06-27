namespace NorthMongo.Domain.People
{
    public class Territory : BaseEntity
    {
        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
