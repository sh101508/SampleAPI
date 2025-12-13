
namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. WebApplicationBuilder (Services)
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // **建議使用標準名稱**
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 2. WebApplication (Middleware Pipeline)
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // 啟用 Swagger UI 
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
