using AutoMapper;
using Application.Dtos.AnalyticsDtos;
using Application.Dtos.BookDtos;
using Application.Dtos.BorrowRecordDtos;
using Application.Dtos.CustomerDtos;
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
