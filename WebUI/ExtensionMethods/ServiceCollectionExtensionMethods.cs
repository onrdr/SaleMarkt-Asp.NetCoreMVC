using Business.Services.Abstract;
using Business.Services.Concrete;
using DataAccess;
using Models.Identity;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Models.Smtp;
using DataAccess.Repositories.Concrete.Cache;

namespace WebUI.ExtensionMethods;

public static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services
            .AddApplicationServices()
            .AddMemoryCache()
            .ConfigureDatabase(builder.Configuration)
            .AddAuthorization()
            .AddIdentity()
            .ConfigureApplicationCookie()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .ConfigureSmtpService(builder);

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, ConfigurationManager configurationManager)
    { 
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configurationManager.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        #region Activate / Deactivate Cached Repos
        services.AddScoped<CategoryRepository>();
        services.AddScoped<ICategoryRepository, CachedCategoryRepository>();

        services.AddScoped<ProductRepository>();
        services.AddScoped<IProductRepository, CachedProductRepository>();

        // services.AddScoped<ICategoryRepository, CategoryRepository>();
        // services.AddScoped<IProductRepository, ProductRepository>();
        #endregion

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
        services.AddScoped<IOrderHeaderService, OrderHeaderService>();

        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();

        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IViewRenderService, ViewRenderService>(); 

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";

            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services)
    {
        CookieBuilder cookieBuilder = new()
        {
            Name = "SaleMarkt",
            HttpOnly = false,
            SameSite = SameSiteMode.Lax,
            SecurePolicy = CookieSecurePolicy.SameAsRequest,
        };

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = new PathString("/Home/Login");
            options.LogoutPath = new PathString("/User/Logout");
            options.AccessDeniedPath = new PathString("/User/AccessDenied");
            options.Cookie = cookieBuilder;
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(60);
        });

        return services;
    }

    private static IServiceCollection ConfigureSmtpService(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
        return services;
    }
}
