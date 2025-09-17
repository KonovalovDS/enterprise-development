using library.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace library.Api.Controllers;

/// <summary>
/// Analytics endpoints for books, customers, and publishers.
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly AnalyticsService _analyticsService;

    /// <summary>
    /// Initializes a new instance of <see cref="AnalyticsController"/>.
    /// </summary>
    public AnalyticsController(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>All borrowed books sorted by name.</summary>
    [HttpGet("borrowed-books")]
    public async Task<IActionResult> GetAllBorrowedBooksSorted()
    {
        var result = await _analyticsService.GetAllBorrowedBooksSortedAsync();
        return Ok(result);
    }

    /// <summary>Top five customers in a given period.</summary>
    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopFiveCustomers([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await _analyticsService.GetTopFiveCustomersAsync(start, end);
        return Ok(result);
    }

    /// <summary>Customers with longest borrows.</summary>
    [HttpGet("longest-borrows")]
    public async Task<IActionResult> GetCustomersWithLongestBorrows()
    {
        var result = await _analyticsService.GetCustomersWithLongestBorrowsAsync();
        return Ok(result);
    }

    /// <summary>Top five publishers in a given period.</summary>
    [HttpGet("top-publishers")]
    public async Task<IActionResult> GetTopFivePublishersLastYear([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await _analyticsService.GetTopFivePublishersLastYearAsync(start, end);
        return Ok(result);
    }

    /// <summary>Top five least popular books.</summary>
    [HttpGet("least-popular-books")]
    public async Task<IActionResult> GetTopFiveLeastPopularBooks()
    {
        var result = await _analyticsService.GetTopFiveLeastPopularBooksAsync();
        return Ok(result);
    }
}