using CoinFlo.BLL.IRepository;
using CoinFlo.BLL.Repository;
using CoinFlo.DAL.DapperDAL;

namespace CoinFlo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<IDapperDataAccess, DapperDataAccess>();
            builder.Services.AddTransient<IUsersRepository, UserRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
            {
                build.WithOrigins("http://localhost:5173")
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("corspolicy");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}