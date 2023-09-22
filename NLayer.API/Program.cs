using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLayer.API;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Concreate;
using NLayer.Core.Services;
using NLayer.Core.Token;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Concreate;
using NLayer.Repository.UnitOfWorks;

using NLayer.Service.Mapping;
using NLayer.Service.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TokenHandler = NLayer.Repository.Token.TokenHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IIHubService, IPHubService>();


builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddDefaultTokenProviders() // Ekle
    .AddEntityFrameworkStores<AppDbContext>() // YourDbContext'e kendi veritabaný baðlamýnýzý ekleyin
    .AddDefaultTokenProviders();// YourDbContext'e kendi veritabaný baðlamýnýzý ekleyin

builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, AppRole>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;

    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.AddSignalR();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.SlidingExpiration = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

//    options.LoginPath = "/Account/Login"; // Giriþ yapma URL'nizi belirtin
//    options.LogoutPath = "/Account/Logout"; // Çýkýþ yapma URL'nizi belirtin

//    options.Cookie = new CookieBuilder()
//    {
//        HttpOnly = true,
//        Name = "Student.Security.Cookie"
//    };
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Roles", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(builder.Configuration["Token:Audience"])),
            ValidIssuer = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(builder.Configuration["Token:Issuer"])),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", p =>
    {
        p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()

    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; // isteðe baðlý olarak derinlik sýnýrýný artýrabilirsiniz
    });

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagmentDb"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);

    });
});

builder.Services.AddSwaggerGen(c =>
{
    // Swagger belgelerine güvenlik þemasýný ekleyin
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    // Swagger belgelerini korumak için yetkilendirme gereksinimini ekleyin
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });

    // JWT yetkilendirme gereksinimini uygulayýn
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});



builder.Host.UseServiceProviderFactory(

    new AutofacServiceProviderFactory()

    );
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//Module start
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kurs Defteri");
    c.RoutePrefix = "api"; // Swagger UI'yi kök dizinde görüntülemek için
    c.DisplayRequestDuration(); // Ýsteðin süresini görüntülemek için
    c.InjectStylesheet("/path/to/custom.css"); // Özel CSS dosyasýný enjekte etmek için
});
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowMyOrigin");
app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UserCustomException();


app.MapControllers();
app.MapHub<IPHubService>("ip-hub");
app.Run();

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .Any();

        if (hasAuthorizeAttribute)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [bearerScheme] = new List<string>()
                }
            };
        }
    }
}

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<StudentDtoValidator>());

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddAutoMapper(typeof(MapProfile));

//builder.Services.AddIdentity<AppUser, AppRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireUppercase = true;

//    options.Lockout.AllowedForNewUsers = true;
//    options.User.RequireUniqueEmail = false;
//    options.SignIn.RequireConfirmedEmail = false;
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.SlidingExpiration = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

//    options.Cookie = new CookieBuilder()
//    {
//        HttpOnly = true,
//        Name = "Student.Security.Cookie"
//    };
//});

//builder.Services.AddAuthorization();

//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.MaxDepth = 64;
//    });

//builder.Services.AddDbContext<AppDbContext>(x =>
//{
//    x.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagmentDb"), options =>
//    {
//        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
//    });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowMyOrigin", p =>
//    {
//        p.AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseCors("AllowMyOrigin");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//app.Run();