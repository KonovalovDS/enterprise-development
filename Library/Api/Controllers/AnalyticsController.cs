using Application.Dtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Analytics endpoints for books, customers, and publishers.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : Controller
{
    /// <summary>
    /// All borrowed books sorted by name.
    /// </summary>
    [HttpGet("borrowed-books")]
    public async Task<IActionResult> GetAllBorrowedBooksSorted([FromQuery] DateOnly date)
    {
        var result = await analyticsService.GetAllBorrowedBooksByDateSortedAsync(date);
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
        var result = await analyticsService.GetTopFiveCustomersAsync(start, end);
        return Ok(result);
    }

    /// <summary>
    /// Customers with longest borrows.
    /// </summary>
    [HttpGet("longest-borrows")]
    public async Task<IActionResult> GetCustomersWithLongestBorrows()
    {
        var result = await analyticsService.GetCustomersWithLongestBorrowsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Top five publishers in a given period.
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
    /// Top five least popular books.
    /// </summary>
    [HttpGet("least-popular-books")]
    public async Task<IActionResult> GetTopFiveLeastPopularBooks()
    {
        var result = await analyticsService.GetTopFiveLeastPopularBooksAsync();
        return Ok(result);
    }
}