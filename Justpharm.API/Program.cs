
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using Justpharm.Library.Areas.Identity.Data;
using Justpharm.Library.Models;
using Justpharm.Library.Repository;


namespace Justpharm.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Culture
            CultureInfo.CurrentCulture = new CultureInfo("es-ES")
            {
                DateTimeFormat = new DateTimeFormatInfo()
                {
                    ShortDatePattern = "dd/MM/yyyy",
                    ShortTimePattern = "HH:mm",
                },
            };
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // ConnectionString
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<JustpharmContext>(f => f.UseSqlServer(connectionString));
            builder.Services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddLog4Net();
            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<IdentityDataContext>();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtIssuer"],
                        ValidAudience = builder.Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!))
                    };
                });

            // Servicios:
            builder.Services.AddScoped<DbQry>();
            
            
            //builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //                .AddRoles<IdentityRole>()
            //                .AddEntityFrameworkStores<IdentityDataContext>();


            builder.Services.AddAuthorization();
            //builder.Services.AddAuthentication("Bearer");


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.Configure<RequestLocalizationOptions>(f =>
            {
                f.DefaultRequestCulture = new RequestCulture(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture);
                f.SetDefaultCulture("es-ES");
            });

            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //});

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //app.MapIdentityApi<IdentityUser>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseCors("AllowAll");


            app.Run();
        }
    }
}
