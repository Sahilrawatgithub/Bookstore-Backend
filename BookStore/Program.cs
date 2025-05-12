
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using StackExchange.Redis;
using RepositoryLayer.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Microsoft.Extensions.Logging;
using ConsumerLayer;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                
                var builder = WebApplication.CreateBuilder(args);
                logger.Info("Application started....");

                // Add services to the container.
                builder.Services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("StartConnection")));

                var redisConfig = builder.Configuration.GetSection("Redis:ConnectionString").Value;

                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(LogLevel.Information);
                builder.Host.UseNLog();


                builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));
                builder.Services.AddScoped(provider =>
                {
                    var connection = provider.GetRequiredService<IConnectionMultiplexer>();
                    return connection.GetDatabase();
                });


                builder.Services.AddScoped<IUserRL, UserImplRl>();
                builder.Services.AddScoped<IUserBL, UserImplBL>();
                
                builder.Services.AddScoped<PasswordHasher>();
                builder.Services.AddScoped<AuthService>();

                builder.Services.AddScoped<IBookRL, BookImplRL>();
                builder.Services.AddScoped<IBookBL, BookImplBL>();

                builder.Services.AddScoped<IAddressRL, AddressImplRL>();
                builder.Services.AddScoped<IAddressBL,AddressImplBL>();

                builder.Services.AddScoped<IOrderRL, OrderImplRL>();
                builder.Services.AddScoped<IOrderBL, OrderImplBL>();

                builder.Services.AddScoped<IWishlistRL, WishlistImplRL>();
                builder.Services.AddScoped<IWishlistBL, WishlistImplBL>();

                builder.Services.AddScoped<ICartRL, CartImplRL>();
                builder.Services.AddScoped<ICartBL, CartImplBL>();
                builder.Services.AddScoped<Publisher>();


                builder.Services.AddHostedService<RabbitMqConsumerService>();

                var jwtKey = builder.Configuration["Jwt:Key"];
                var jwtIssuer = builder.Configuration["Jwt:Issuer"];
                var jwtAudience = builder.Configuration["Jwt:Audience"];

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });


                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore", Version = "v1" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Enter JWT as: {your token}",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                               {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference
                                   {
                                       Type = ReferenceType.SecurityScheme,
                                       Id = JwtBearerDefaults.AuthenticationScheme
                                   }
                               },
                               Array.Empty<string>()
                           }
                               });
                });

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        policy =>
                        {
                                  policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        });
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseCors("AllowAll");
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
