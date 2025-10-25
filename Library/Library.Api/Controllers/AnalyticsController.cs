using Microsoft.AspNetCore.Mvc;
using Library.Application.Services;
using Library.Application.Contracts.AnalyticsDtos;

namespace Library.Api.Controllers;

/// <summary>
/// Analytics endpoints for books, customers, and publishers.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Returns all books borrowed on a current date, sorted by their title.
    /// </summary>
    [HttpGet("borrowed-books")]
    public async Task<ActionResult<List<BookWithBorrowCountDto>>> GetAllBorrowedBooksSorted()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var result = await analyticsService.GetAllBorrowedBooksByDateSortedAsync(date);
        return Ok(result);
    }

    // <summary>
    /// Returns the top five customers based on borrow count within a specified date range.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    [HttpGet("top-customers")]
    public async Task<ActionResult<List<CustomerWithBorrowCountDto>>> GetTopFiveCustomers([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var result = await analyticsService.GetTopFiveCustomersAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Returns customers with the longest borrowing duration across all records.
    /// </summary>
    [HttpGet("longest-borrows")]
    public async Task<ActionResult<List<CustomerWithDurationDto>>> GetCustomersWithLongestBorrows()
    {
        var result = await analyticsService.GetCustomersWithLongestBorrowsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Returns the top five publishers with the highest book borrow count within last year.
    /// </summary>
    [HttpGet("top-publishers")]
    public async Task<ActionResult<List<PublisherDto>>> GetTopFivePublishersLastYear()
    {
        var end = DateOnly.FromDateTime(DateTime.Now);
        var start = end.AddYears(-1);
        var result = await analyticsService.GetTopFivePublishersByDateAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Returns the top five least popular books based on borrow frequency within last year.
    /// </summary>
    [HttpGet("least-popular-books")]
    public async Task<ActionResult<List<BookWithBorrowCountDto>>> GetTopFiveLeastPopularBooksLastYear()
    {
        var end = DateOnly.FromDateTime(DateTime.Now);
        var start = end.AddYears(-1);
        var result = await analyticsService.GetTopFiveLeastPopularBooksAsync(start, end);
        return Ok(result);
    }
}