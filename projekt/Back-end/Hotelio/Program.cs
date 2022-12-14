using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Hotelio.Auth;
using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsApi",
        builder => builder.WithOrigins("http://localhost:3000", "https://hoteliofront.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddDbContext<CrudContext>(option => option.UseSqlServer("name=ConnectionStrings:HotelioDatabase"));


//auth
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<CrudContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireUppercase = false;
});
//services
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<AdditionalServiceService>(); 
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DbSeeder>();
builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequestResourceOwner",
        policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});

builder.Services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "front";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsApi");
app.UseSpaStaticFiles();

app.MapWhen(req => !req.Request.Path.Value.StartsWith("/api"), appBuilder =>
{
    appBuilder.UseSpa(spa =>
    {
        spa.Options.SourcePath = Path.Combine(builder.Environment.ContentRootPath, "front");

        if (builder.Environment.IsDevelopment())
        {
            spa.UseReactDevelopmentServer("start");
        }
    });
});


app.Use(async (req, next) =>
{
    var headers = req.Request.Headers.ToList();

    foreach (var item in headers)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }

    await next();
});

app.UseAuthentication();// this first
app.UseAuthorization();

app.MapControllers();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<DbSeeder>();
await dbSeeder.SeedAsync();

app.Run();