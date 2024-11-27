using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> Search(string searchTerm,
    int pageNumber = 1,
    int pageSize = 4)
    {
        var query = DB.PagedSearch<Item>();
        query.Sort(i => i.Ascending(i => i.Make));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(MongoDB.Entities.Search.Full, searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
