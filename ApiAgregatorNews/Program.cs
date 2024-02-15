
using ApiAgregatorNews.Data;
using ApiAgregatorNews.Services;
using ApiAgregatorNews.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace ApiAgregatorNews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure EF DBContext Service (Database)

            builder.Services.AddDbContext<ApiAgregatorNewsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["DatabaseOptions:ConnectionString"]);

            });

            #endregion

            #region Configure Services

            builder.Services.AddSingleton<IItemService, ItemService>();
            builder.Services.AddSingleton<ISourceService, SourceService>();

            #endregion

            // Add services to the container.

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
