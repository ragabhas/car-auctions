using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
        await DB.Index<Item>()
          .Key(i => i.Make, KeyType.Text)
          .Key(i => i.Model, KeyType.Text)
          .Key(i => i.Color, KeyType.Text)
          .CreateAsync();

        var count = await DB.CountAsync<Item>();

        if (count == 0)
        {
            var itemsData = File.ReadAllText("Data/auctions.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var items = JsonSerializer.Deserialize<List<Item>>(itemsData, options);

            await DB.SaveAsync(items);
        }
    }
}
