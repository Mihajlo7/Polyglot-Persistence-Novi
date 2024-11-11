using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
