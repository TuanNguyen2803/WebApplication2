using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication2.Models
{
    public class Number
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Value { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
    }
}
