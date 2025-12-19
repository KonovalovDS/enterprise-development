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

builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddScoped<AuthorizedHttpClientHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7154/");
})
    .AddHttpMessageHandler<AuthorizedHttpClientHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

builder.Services.AddScoped<IBaseApiService<BookGetDto, BookEditDto>>(sp =>
    new BaseApiService<BookGetDto, BookEditDto>(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        "books"
    ));
builder.Services.AddScoped<IBaseApiService<CustomerGetDto, CustomerEditDto>>(sp =>
    new BaseApiService<CustomerGetDto, CustomerEditDto>(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        "customers"
    ));
builder.Services.AddScoped<IBaseApiService<BorrowRecordGetDto, BorrowRecordEditDto>>(sp =>
    new BaseApiService<BorrowRecordGetDto, BorrowRecordEditDto>(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        "records"
    ));

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
