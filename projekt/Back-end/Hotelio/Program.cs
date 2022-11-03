using System.Text;
using System.Text.Json.Serialization;
using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hotelio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            });

            builder.Services.AddScoped<HotelService>(); // here be adding services
            builder.Services.AddScoped<RoomService>();
            builder.Services.AddScoped<AdditionalServiceService>(); 
            builder.Services.AddScoped<UserService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();// this first
            app.UseAuthorization();

            app.MapControllers();

            app.Services.CreateScope().ServiceProvider.GetRequiredService<DbSeeder>().SeedAsync(); //??

            app.Run();
        }
    }
}