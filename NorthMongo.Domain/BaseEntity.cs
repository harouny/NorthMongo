using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NorthMongo.Domain
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
