using AutoMapper;
using Library.Application.Contracts.AnalyticsDtos;
using Library.Application.Contracts.BookDtos;
using Library.Application.Contracts.BorrowRecordDtos;
using Library.Application.Contracts.CustomerDtos;
using Library.Domain.Entities;

namespace Library.Application.Mappers;

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
        CreateMap<BookEditDto, Book>().ReverseMap();
        CreateMap<BookGetDto, Book>().ReverseMap();

        CreateMap<CustomerEditDto, Customer>().ReverseMap();
        CreateMap<CustomerGetDto, Customer>().ReverseMap();

        CreateMap<BorrowRecordEditDto, BorrowRecord>().ReverseMap();
        CreateMap<BorrowRecordGetDto, BorrowRecord>().ReverseMap();

        CreateMap<BookWithBorrowCountDto, Book>().ReverseMap();
        CreateMap<CustomerWithBorrowCountDto, Customer>().ReverseMap();
        CreateMap<CustomerWithDurationDto, Customer>().ReverseMap();
    }
}
