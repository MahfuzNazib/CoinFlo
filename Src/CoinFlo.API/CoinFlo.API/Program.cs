using CoinFlo.API.Helpers;
using CoinFlo.BLL.IRepository.IAuthRepository;
using CoinFlo.BLL.IRepository.ICategoryRepository;
using CoinFlo.BLL.IRepository.IPayTypeRepository;
using CoinFlo.BLL.IRepository.IUsersRepository;
using CoinFlo.BLL.Repository.AuthRepository;
using CoinFlo.BLL.Repository.CategoryRepository;
using CoinFlo.BLL.Repository.PayTypeRepository;
using CoinFlo.BLL.Repository.UserRepository;
using CoinFlo.DAL.DapperDAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            builder.Services.AddTransient<IAuthRepository, AuthRepository>();
            builder.Services.AddTransient<IUsersRepository, UserRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<IPayTypeRepository, PayTypeRepository>();
            builder.Services.AddSingleton<JwtTokenGenerator>();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddCors(options => options.AddPolicy("corspolicy", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:5173")
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

            app.UseHttpsRedirection();

            app.UseCors("corspolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            ResponseHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

            app.MapControllers();

            app.Run();
        }
    }
}
