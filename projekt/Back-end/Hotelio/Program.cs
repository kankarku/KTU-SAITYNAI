using Hotelio.Context;
using Hotelio.Services;
using Microsoft.EntityFrameworkCore;

namespace Hotelio
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<CrudContext>(option => option.UseSqlServer("name=ConnectionStrings:HotelioDatabase"));
            builder.Services.AddScoped<HotelService>(); // here be adding services
            builder.Services.AddScoped<RoomService>(); // here be adding services
            builder.Services.AddScoped<AdditionalServiceService>(); // here be adding services
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}