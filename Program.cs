using MongoDB.Driver;
using TodoApp.Configurations;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MongoDB Settings to Configuration
builder.Services.Configure<MongoConfigurations>(builder.Configuration.GetSection("MongoConfigurations"));

// 2. Register MongoClient as a Singleton
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoConfigurations>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// 3. Register the MongoDB Database Instance
builder.Services.AddScoped(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoConfigurations>>().Value;
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
