using DiscountSvc_Provider.Models;
using DiscountSvc_Provider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountSvc_Provider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<DiscountService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var svc = endpoints.ServiceProvider.GetRequiredService<DiscountService>();
                endpoints.MapPost("/discount", async context =>
                {
                    var model = await context.Request.ReadFromJsonAsync<DiscountModel>();
                    var amount = svc.GetDiscountAmount(model.CustomerRating);

                    await context.Response.WriteAsJsonAsync(new DiscountModel
                    {
                        CustomerRating = model.CustomerRating,
                        AmountToDiscount = amount
                    });
                });
            });

            app.Run();
        }
    }
}
