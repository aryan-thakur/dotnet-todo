namespace TodoApp.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class TodoModel
{   
    // Required otherwise very hard to differentiate items
    [BsonId]  // Specifies that this is the primary key (_id field in MongoDB)
    [BsonRepresentation(BsonType.ObjectId)]  // Tells MongoDB to treat this as an ObjectId even if it's a string
    public string _id { get; set; }

    [BsonElement("title")] 
    public string Title { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    public TodoModel (string title = "Unknown", string desc = "")  {
        this.Title = title;
        this.Description = desc;
    }

    public TodoModel ()  { }
}
