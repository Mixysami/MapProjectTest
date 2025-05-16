using BLL;
using Core.Interfaces;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Serilog;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "ClientApp/build";
            });
            builder.Host.UseSerilog();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IMapManager, MapService>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseReactDevelopmentServer(npmScript: "start");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}