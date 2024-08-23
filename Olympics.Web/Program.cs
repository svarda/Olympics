using Olympics.Web;
using Olympics.Web.Data;
using Olympics.Web.Grid;
using SoloX.BlazorJsonLocalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped(client => new HttpClient { });
builder.Services.AddSingleton<MedalDataService>();
builder.Services.AddScoped<IMedalFilters, GridControls>();

string[] cultures = ["en-US", "fr-FR"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(cultures[0])
    .AddSupportedCultures(cultures)
    .AddSupportedUICultures(cultures);

builder.Services.AddJsonLocalization(
    builder => builder.UseEmbeddedJson(
        options => options.ResourcesPath = "Resources"),
        ServiceLifetime.Singleton);

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
