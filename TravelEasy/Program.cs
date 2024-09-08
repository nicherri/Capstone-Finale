using Microsoft.AspNetCore.Http.Features;  // Aggiungi questa importazione per FormOptions
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configura il servizio per il DbContext
builder.Services.AddDbContext<TravelEasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFAQService, FAQService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IShelvingService, ShelvingService>();
builder.Services.AddScoped<IShelfService, ShelfService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IWishlistItemService, WishlistItemService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();

// Configura i controller con opzioni JSON
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Imposta il limite di upload a 100 MB per i file multimediali
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});

// Aggiungi altri servizi necessari
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura il middleware della pipeline di richiesta
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
