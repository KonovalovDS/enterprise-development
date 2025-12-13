using Library.Client;
using Library.Client.Models.BookDtos;
using Library.Client.Models.BorrowRecordDtos;
using Library.Client.Models.CustomerDtos;
using Library.Client.Models.Interfaces;
using Library.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
