using API.Models.Data;
using API.Models.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //k connection string
            var connectionString = builder.Configuration
            .GetConnectionString("DefaultConnection");
            //k Dbcontext service and connecting to mssqlserver using the connection string
            builder.Services.AddDbContext<DirectoryContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddScoped<DocumentsRepository>();

            builder.Services.Configure<FormOptions>
                (o =>
                {
                    o.ValueLengthLimit = int.MaxValue;
                    o.MultipartBoundaryLengthLimit = int.MaxValue;  
                    o.MemoryBufferThreshold = int.MaxValue; 
                });
            //builder.Services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //); 
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            using (var context = builder.Services.BuildServiceProvider()
                    .GetRequiredService<DirectoryContext>())
            {
                var loggerFactory = builder.Services.BuildServiceProvider()
                    .GetRequiredService<ILoggerFactory>();
                try
                {
                    //k will create a db if it doesn't exist
                    //await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "an error occured");
                }
            }
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseStaticFiles();
            //app.UseStaticFiles( new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path
            //    .Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}