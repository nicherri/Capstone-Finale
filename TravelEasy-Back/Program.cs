using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura il servizio CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Permetti solo richieste dal frontend Angular
                  .AllowAnyHeader()                     // Permetti qualsiasi header
                  .AllowAnyMethod();                    // Permetti tutti i metodi HTTP (GET, POST, PUT, DELETE, ecc.)
        });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configura il servizio per il DbContext
builder.Services.AddDbContext<TravelEasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrazione dei servizi
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

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});

var app = builder.Build();

// Configura il middleware della pipeline di richiesta
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Middleware per servire i file statici dalla cartella wwwroot
app.UseStaticFiles();

// Aggiungi qui il middleware per servire file statici da una cartella personalizzata
app.UseStaticFiles();

app.UseRouting();

// Abilita CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


