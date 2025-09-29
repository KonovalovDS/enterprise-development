using Microsoft.AspNetCore.Mvc;

using Application.Services;

namespace Api.Controllers;

/// <summary>
/// Analytics endpoints for books, customers, and publishers.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Returns all books borrowed on a specific date, sorted by their title.
    /// </summary>
    /// <param name="date">The date for which borrowed books are requested.</param>
    [HttpGet("borrowed-books")]
    public async Task<IActionResult> GetAllBorrowedBooksSorted([FromQuery] DateOnly date)
    {
        var result = await analyticsService.GetAllBorrowedBooksByDateSortedAsync(date);
        return Ok(result);
    }

    // <summary>
    /// Returns the top five customers based on borrow count within a specified date range.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopFiveCustomers([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await analyticsService.GetTopFiveCustomersAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Returns customers with the longest borrowing duration across all records.
    /// </summary>
    [HttpGet("longest-borrows")]
    public async Task<IActionResult> GetCustomersWithLongestBorrows()
    {
        var result = await analyticsService.GetCustomersWithLongestBorrowsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Returns the top five publishers with the highest book borrow count within a specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    [HttpGet("top-publishers")]
    public async Task<IActionResult> GetTopFivePublishersByDate([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await analyticsService.GetTopFivePublishersByDateAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Returns the top five least popular books based on borrow frequency.
    /// </summary>
    [HttpGet("least-popular-books")]
    public async Task<IActionResult> GetTopFiveLeastPopularBooks()
    {
        var result = await analyticsService.GetTopFiveLeastPopularBooksAsync();
        return Ok(result);
    }
}