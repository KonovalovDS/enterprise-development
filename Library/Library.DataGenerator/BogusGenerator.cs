using Bogus;
using Library.Application.Contracts.BookDtos;
using Library.Application.Contracts.BorrowRecordDtos;
using Library.Application.Contracts.CustomerDtos;

namespace Library.DataGenerator;

/// <summary>
/// Data generator for the library including books, customers and borrow records contracts.
/// </summary>
public class BogusGenerator
{
    /// <summary>
    /// Faker generator for books contracts.
    /// </summary>
    private readonly Faker<BookEditDto> _booksFaker;

    /// <summary>
    /// Faker generator for customers contracts.
    /// </summary>
    private readonly Faker<CustomerEditDto> _customersFaker;

    /// <summary>
    /// Faker generator for borrow records contracts.
    /// </summary>
    private readonly Faker<BorrowRecordEditDto> _recordsFaker;

    /// <summary>
    /// books counter including seeded data in db & generated data.
    /// </summary>
    private int _booksCount = 1;

    /// <summary>
    /// customers counter including seeded data in db & generated data.
    /// </summary>
    private int _customersCount = 1;

    /// <summary>
    /// Initializes an instance of <see cref="BogusGenerator"/> and sets up Faker rules for each entity.
    /// </summary>
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

        _customersFaker = new Faker<CustomerEditDto>()
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+7##########"));

        _recordsFaker = new Faker<BorrowRecordEditDto>()
            .RuleFor(x => x.BookId, f => f.Random.Int(1, _booksCount))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(1, _customersCount))
            .RuleFor(x => x.BorrowDate, f => DateOnly.FromDateTime(f.Date.Past(1)))
            .RuleFor(x => x.BorrowDuration, f => f.Random.Int(1, 90));
    }

    /// <summary>
    /// Generates a new <see cref="BookEditDto"/> instance.
    /// </summary>
    /// <returns>A new book contract instance</returns>
    public BookEditDto GenerateBook() 
    {
        _booksCount++;
        return _booksFaker.Generate();
    }

    /// <summary>
    /// Generates a new <see cref="CustomerEditDto"/> instance.
    /// </summary>
    /// <returns>A new customer contract.</returns>
    public CustomerEditDto GenerateCustomer()
    {
        _customersCount++;
        return _customersFaker.Generate();
    }

    /// <summary>
    /// Generates a new <see cref="BorrowRecordEditDto"/> instance with valid existing book and customer IDs.
    /// </summary>
    /// <returns>A new borrow record contract.</returns>
    public BorrowRecordEditDto GenerateRecord() => _recordsFaker.Generate();
}