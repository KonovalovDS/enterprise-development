using Bogus;
using Library.Application.Contracts.BookDtos;
using Library.Application.Contracts.BorrowRecordDtos;
using Library.Application.Contracts.CustomerDtos;

namespace Library.DataGenerator;
public class BogusGenerator
{
    private readonly Faker<BookEditDto> _booksFaker;
    private readonly Faker<CustomerEditDto> _customerFaker;
    private readonly Faker<BorrowRecordEditDto> _recordsFaker;

    public BogusGenerator()
    {
        var publishers = new[]
        {
            "NorthernWord", 
            "BookMosaic", 
            "LiteraryWind", 
            "GoldenPage", 
            "EchoOfThought", 
            "TheBinding", 
            "IntellectPublishing", 
            "WhiteLine", 
            "BeaconPress", 
            "NewEraPublishing"
        };

        var types = new[] {
            "Hardcover",
            "Paperback",
            "Ebook",
            "Audiobook",
            "LimitedEdition"
        };

        _booksFaker = new Faker<BookEditDto>()
            .RuleFor(x => x.InventoryNumber, f => f.Random.AlphaNumeric(6).ToUpper())
            .RuleFor(x => x.Code, f => f.Random.AlphaNumeric(2).ToUpper())
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Publisher, f => f.PickRandom(publishers))
            .RuleFor(x => x.PublishingType, f => f.PickRandom(types))
            .RuleFor(x => x.PublicationYear, f => f.Random.Int(1900, 2025));

        _customerFaker = new Faker<CustomerEditDto>()
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+7##########"));

        _recordsFaker = new Faker<BorrowRecordEditDto>()
            .RuleFor(x => x.BookId, f => f.Random.Int(1, 20))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(1, 20))
            .RuleFor(x => x.BorrowDate, f => DateOnly.FromDateTime(f.Date.Past(1)))
            .RuleFor(x => x.BorrowDuration, f => f.Random.Int(1, 90));
    }

    public BookEditDto GenerateBook() => _booksFaker.Generate();
    public CustomerEditDto GenerateCustomer() => _customerFaker.Generate();
    public BorrowRecordEditDto GenerateRecord() => _recordsFaker.Generate();
}
