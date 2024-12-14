using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerAPI.Models
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}