using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> Search(string searchTerm)
    {
        var query = DB.Find<Item>();
        query.Sort(i => i.Ascending(i => i.Make));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(MongoDB.Entities.Search.Full, searchTerm).SortByTextScore();
        }

        var result = await query.ExecuteAsync();

        return result;
    }
}
