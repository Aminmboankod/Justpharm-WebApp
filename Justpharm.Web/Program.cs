using Justpharm.Web.Components;
using Justpharm.Web.Components.Account;
using Justpharm.Web.Data;
using Justpharm.Web.Models;
using Justpharm.Web.Repository;
using Justpharm.Web.Services;
using Justpharm.Web.Services.Email;
using Justpharm.Web.Services.Notification;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Justpharm.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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

            builder.Services.AddDataProtection();
            builder.Services.Configure<RequestLocalizationOptions>(f =>
            {
                f.DefaultRequestCulture = new RequestCulture(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture);
                f.SetDefaultCulture("es-ES");
            });


            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSyncfusionBlazor().ConfigureHttpJsonOptions(f => f.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetValue<string>("RegisterLicense"));

            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            builder.Services.AddLog4Net();
            builder.Services.AddSingleton<DbQry>();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<JustpharmContext>(f => f.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddSignInManager()
                            .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();
            builder.Services.AddScoped<CookiesProvider>();
            builder.Services.AddScoped<CookieEvents>();
            builder.Services.AddScoped<ICuidadoService, CuidadoService>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(CookieEvents);
            });

            builder.Services.AddEmailSender();
            builder.Services.AddNotifications();
            builder.Services.AddHostedService<AvisoTomasService>();
            var p = builder.Configuration.GetValue<string>("UriString");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(p) });
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
            app.MapAdditionalIdentityEndpoints();


            using (IServiceScope? scope = app.Services.CreateScope())
            {
                RoleManager<IdentityRole>? roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<ApplicationUser>? userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // el rol "Invitado" será algo especial
                // el rol "Usuario" es el rol por defecto para los usuarios de la aplicación
                string[]? roleNames = new[] { "Administrador", "Usuario", "Invitado", "API", "Técnico", "Paciente", "Encargado" };
                // crear los roles
                foreach (string roleName in roleNames)
                    if (!(await roleManager.RoleExistsAsync(roleName)))
                        await roleManager.CreateAsync(new IdentityRole(roleName));

                // crear el usuario por si no existe
                ApplicationUser? adminUser = new ApplicationUser
                {
                    UserName = app.Configuration.GetValue<string>("DefaultAdmin:UserName"),
                    Email = app.Configuration.GetValue<string>("DefaultAdmin:Email"),
                    EmailConfirmed = true
                };
                adminUser.NormalizedUserName = adminUser.UserName!.ToUpper();
                adminUser.NormalizedEmail = adminUser.Email!.ToUpper();

                ApplicationUser? user = await userManager.FindByEmailAsync(adminUser.Email!);
                if (user == null)
                {
                    IdentityResult? result = await userManager.CreateAsync(adminUser, app.Configuration.GetValue<string>("DefaultAdmin:Password")!);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(adminUser, ["Administrador", "API"]);
                    }
                    else
                    {
                        StringBuilder? sb = new StringBuilder();
                        foreach (IdentityError error in result.Errors)
                        {
                            sb.AppendLine(error.Description);
                        }
                        throw new InvalidOperationException(sb.ToString());
                    }
                }

                Statics.MaestroPerfiles = scope.ServiceProvider.GetRequiredService<DbQry>().All<_MaestroPerfil>().OrderBy(c => c.Tipo).ToList();
                Statics.UsuarioExternosTipo = scope.ServiceProvider.GetRequiredService<DbQry>().All<_MaestroPerfil>(c => c.Ref > 2).OrderBy(c => c.Tipo).ToList();
                Statics.ListadoDietas = scope.ServiceProvider.GetRequiredService<DbQry>().All<_MaestroDietas>().OrderBy(c => c.Orden).ToList();
                Statics.ListadoNecesidades = scope.ServiceProvider.GetRequiredService<DbQry>().All<_MaestroNecesidades>().OrderBy(c => c.Orden).ToList();
                Statics.ListadoCuidadosCategoria = scope.ServiceProvider.GetRequiredService<DbQry>().All<_MaestroCuidados>().OrderBy(c => c.Orden).ToList();
            }

            app.Run();
        }
    }
}
