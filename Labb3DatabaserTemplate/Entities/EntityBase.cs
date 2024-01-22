using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataAccess.Entities;

public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}