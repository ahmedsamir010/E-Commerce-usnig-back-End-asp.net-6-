using API_01.Extention;
using API_01.MiddleWare;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Core.Entities.EmailSettings;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Identity.SeedUsers;
using Talabat.Service;
namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            builder.Services.AddSingleton<IConnectionMultiplexer>(s =>
            {
                var configuration = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configuration);
            });


            builder.Services.AddControllers(); // API Services

            builder.Services.AddSwagerServices();
            builder.Services.AddApplicationSevices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().WithOrigins(builder.Configuration["FrontBaseYrl"]);
                });
            });


            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IEmailService, EmailSettings>();
            builder.Services.Configure<TwilloSettings>(builder.Configuration.GetSection("Twillio"));
            builder.Services.AddTransient<ISmsServeice, SmsService>();
            #endregion

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var dbContext = services.GetRequiredService<StoreContext>();
                    await dbContext.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(dbContext);

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }

            app.UseMiddleware<ExceptionMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
