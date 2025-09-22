using library.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace library.Api.Controllers;

/// <summary>
/// Analytics endpoints for books, customers, and publishers.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : Controller
{
    /// <summary>
    /// Service for performing analytics operations.
    /// </summary>
    private readonly AnalyticsService _analyticsService = analyticsService;

    /// <summary>
    /// All borrowed books sorted by name.
    /// </summary>
    [HttpGet("borrowed-books")]
    public async Task<IActionResult> GetAllBorrowedBooksSorted()
    {
        var result = await _analyticsService.GetAllBorrowedBooksSortedAsync();
        return Ok(result);
    }

    /// <summary>
    /// Top five customers in a given period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopFiveCustomers([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await _analyticsService.GetTopFiveCustomersAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Customers with longest borrows.
    /// </summary>
    [HttpGet("longest-borrows")]
    public async Task<IActionResult> GetCustomersWithLongestBorrows()
    {
        var result = await _analyticsService.GetCustomersWithLongestBorrowsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Top five publishers in a given period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    [HttpGet("top-publishers")]
    public async Task<IActionResult> GetTopFivePublishersLastYear([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await _analyticsService.GetTopFivePublishersLastYearAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Top five least popular books.
    /// </summary>
    [HttpGet("least-popular-books")]
    public async Task<IActionResult> GetTopFiveLeastPopularBooks()
    {
        var result = await _analyticsService.GetTopFiveLeastPopularBooksAsync();
        return Ok(result);
    }
}