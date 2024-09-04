using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using TodoApp.Models;

namespace TodoApp.Controllers;

public class TodoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<TodoModel> _todoCollection;

    public TodoController(ILogger<HomeController> logger, IMongoDatabase database)
    {
        _database = database;
        _logger = logger;
        _todoCollection = database.GetCollection<TodoModel>("TodoItems");
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    // POST: /Todo/Create
    [HttpPost]
    public IActionResult Create(TodoModel newItem)
    {
        _todoCollection.InsertOne(newItem);  // Insert the new Todo item into the collection
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(string id){
        var document = _todoCollection.Find(Builders<TodoModel>.Filter.Eq("_id", ObjectId.Parse(id))).FirstOrDefault(); //Filter is done through Builders
        return View(document);
    }

    [HttpPost]
    public IActionResult Update(TodoModel newTodo){
        var document = _todoCollection.UpdateOne(Builders<TodoModel>.Filter.Eq("_id", ObjectId.Parse(newTodo._id)), Builders<TodoModel>.Update
    .Set(todoItem => todoItem.Title, newTodo.Title).Set(todoItem => todoItem.Description, newTodo.Description)); // UpdateOne takes in a filter and an update command
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string id){
        var filter = Builders<TodoModel>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = _todoCollection.DeleteOne(filter);
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        var documents = _todoCollection.Find(Builders<TodoModel>.Filter.Empty).Limit(5).ToList();
        return View(documents);
    }
    
    public IActionResult Hello()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
