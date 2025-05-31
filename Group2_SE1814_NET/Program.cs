using CloudinaryDotNet;
using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.Repositories;
using Group2_SE1814_NET.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google"; // Default callback path
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Cấu hình dịch vụ DbContext cho Entity Framework Core
builder.Services.AddDbContext<WebkinhdoanhquanaoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Cấu hình các dịch vụ tùy chỉnh (Repository, Services, etc.)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<GHNService>();

// Thêm các dịch vụ khác cần thiết cho ứng dụng (MVC, Razor Pages, etc.)
builder.Services.AddControllersWithViews();

// Thêm GHN API
builder.Services.AddHttpClient<IGHNService, GHNService>();
builder.Services.AddScoped<IGHNService, GHNService>();
// VNpay
builder.Services.AddHttpClient<IVnPayService, VnPayService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
//Payos
builder.Services.Configure<PayOSConfig>(builder.Configuration.GetSection("PayOS"));
builder.Services.AddHttpClient<IPayosService, PayosService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPayosService, PayosService>();


//Mail API
builder.Services.AddTransient<IEmailServices, EmailServices>();

// Thêm Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Cloudinary
var cloudinarySettings = builder.Configuration.GetSection("Cloudinary");
var cloudinaryAccount = new Account(
    cloudinarySettings["CloudName"],
    cloudinarySettings["ApiKey"],
    cloudinarySettings["ApiSecret"]
);
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

var app = builder.Build();

// Sử dụng Session
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

//config area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
       );

    // Default route for controllers outside of areas
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
