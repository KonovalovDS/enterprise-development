using AutoMapper;

using Application.Dtos;
using Domain.Entities;

namespace Application.Mappers;

/// <summary>
/// AutoMapper profile for mapping between domain entities and their DTOs.
/// </summary>
public class AppMappingProfile : Profile
{
    /// <summary>
    /// Initializes the mappings between DTOs and domain entities.
    /// </summary>
    public AppMappingProfile()
    {
        CreateMap<BookDto, Book>();
        CreateMap<Book, BookDto>();

        CreateMap<CustomerDto, Customer>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<BorrowRecordDto, BorrowRecord>();
        CreateMap<BorrowRecord, BorrowRecordDto>();
    }
}
