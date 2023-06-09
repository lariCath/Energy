using Energy.Service;
using Refit;
using Serilog;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddSingleton<ApiService>();
    builder.Services.AddSingleton<OverviewHandler>();
    builder.Services.AddSingleton<CalculationService>();

    builder.Services.AddRefitClient<IEnergyApi>()
                          .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetConnectionString("APIEnergyCharts") ?? ""));

    builder.Services.AddRefitClient<IWeatherApi>()
                          .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetConnectionString("APIWeatherForecast") ?? ""));

    builder.Services.AddScoped<DialogService>();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
