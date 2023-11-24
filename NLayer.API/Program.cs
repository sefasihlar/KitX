using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IIHubService, IPHubService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();

builder.Services.AddHostedService<DataCleanupService>();




builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddDefaultTokenProviders() // Ekle
    .AddEntityFrameworkStores<AppDbContext>() // YourDbContext'e kendi veritaban� ba�lam�n�z� ekleyin
    .AddDefaultTokenProviders();// YourDbContext'e kendi veritaban� ba�lam�n�z� ekleyin

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

//    options.LoginPath = "/Account/Login"; // Giri� yapma URL'nizi belirtin
//    options.LogoutPath = "/Account/Logout"; // ��k�� yapma URL'nizi belirtin

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
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    
    .WriteTo.File("logs/log.text")
    .Filter.ByIncludingOnly(evt =>
            evt.Properties.ContainsKey("UserName") &&
            evt.Properties["UserName"].ToString() != null
        )
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("StudentManagmentDb"),
        "logs",
        
        columnOptions: new ColumnOptions
        {
            AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn
                {
                    ColumnName = "UserName",
                    DataType = SqlDbType.NVarChar, // Özel sütunun veri tipini belirtin
                    DataLength = 100 // Özel sütunun uzunluğunu belirtin (isteğe bağlı)
                }
            }
        },



        autoCreateSqlTable: true
    )
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog(log);










builder.Services.AddControllers()

    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; // iste�e ba�l� olarak derinlik s�n�r�n� art�rabilirsiniz
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
    // Swagger belgelerine g�venlik �emas�n� ekleyin
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    // Swagger belgelerini korumak i�in yetkilendirme gereksinimini ekleyin
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

    // JWT yetkilendirme gereksinimini uygulay�n
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});




builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});



builder.Host.UseServiceProviderFactory(

    new AutofacServiceProviderFactory()

    );


//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//Module start
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Di�er kay�tlar� ekleyin
    containerBuilder.RegisterModule(new RepoServiceModule());

    // WindowNavigatorGeolocation s�n�f�n� kaydedin

});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kurs Defteri");
    c.RoutePrefix = "api"; // Swagger UI'yi k�k dizinde g�r�nt�lemek i�in
    c.DisplayRequestDuration(); // �ste�in s�resini g�r�nt�lemek i�in
    c.InjectStylesheet("/path/to/custom.css"); // �zel CSS dosyas�n� enjekte etmek i�in
});
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseStaticFiles();

app.UseRouting();


//app.Use(async (context, next) =>
//{
//    var username = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;

//    LogContext.PushProperty("UserName", username);

//    await next();
//});

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;

    LogContext.PushProperty("UserName", username);

    await next();
});




app.UseCors("AllowMyOrigin");

app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UserCustomException();

app.UseSerilogRequestLogging();
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

public class UserNameFilter : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent)
    {
        // Kullanıcı adı alanını kontrol et, boşsa logu atla
        var userName = logEvent.Properties["UserName"]?.ToString();
        return !string.IsNullOrEmpty(userName);
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