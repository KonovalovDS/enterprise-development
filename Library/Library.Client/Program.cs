using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Library.Client;
using Library.Client.Models.Interfaces;
using Library.Client.Services;
using Library.Client.Models.BookDtos;
using Library.Client.Models.CustomerDtos;
using Library.Client.Models.BorrowRecordDtos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7154/") });

builder.Services.AddScoped<IBaseApiService<BookGetDto, BookEditDto>>(sp =>
    new BaseApiService<BookGetDto, BookEditDto>(
        sp.GetRequiredService<HttpClient>(),
        "books"
    ));
builder.Services.AddScoped<IBaseApiService<CustomerGetDto, CustomerEditDto>>(sp =>
    new BaseApiService<CustomerGetDto, CustomerEditDto>(
        sp.GetRequiredService<HttpClient>(),
        "customers"
    ));
builder.Services.AddScoped<IBaseApiService<BorrowRecordGetDto, BorrowRecordEditDto>>(sp =>
    new BaseApiService<BorrowRecordGetDto, BorrowRecordEditDto>(
        sp.GetRequiredService<HttpClient>(),
        "records"
    ));

await builder.Build().RunAsync();
