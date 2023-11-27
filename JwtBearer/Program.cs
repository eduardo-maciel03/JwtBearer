using JwtBearer.Models;
using JwtBearer.Services;

namespace JwtBearer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // injeção de dependência
            builder.Services.AddTransient<TokenService>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            // configurando e apresentando token
            app.MapGet("/", (TokenService service) 
                => service.Generate(
                    new User(
                        1,
                        "teste@hotmail.com", 
                        "123", 
                        new string[]
                        {
                            "student",
                            "premium"
                        }
                    )));

            app.Run();
        }
    }
}
