using AutoMapper;

using Application.Dtos;
using Domain.Entities;

namespace Application.Mappers;

public class AppMappingProfile : Profile
{
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
