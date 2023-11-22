using EShop.DataContext;
using EShop.IdentityService.Helper;
using EShop.IdentityService.Identity;
using EShop.IdentityService.Infrastructure.Authorizaion.Requirements;
using EShop.Model;
using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EShop.IdentityService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIdentityervices(this IServiceCollection services, string? connectionStringConfigName)
        {
            services.AddDbContext<UserIdentityContext>(options =>
            {
                options.UseSqlServer(connectionStringConfigName);
            });
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IdentityBuilder AddIdentityOptions(this IServiceCollection services)
        {
            return services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // configuration can be written here:
                // builder.IdentityService.Configure<IdentityOptions>
                options.SignIn.RequireConfirmedAccount = true;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UserIdentityContext>();
        }

        public static IServiceCollection AddApplicationCookieAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "EShopAuthCookie";
                    // Cookie settings
                    // configuration can be written here:
                    // builder.IdentityService.ConfigureApplicationCookie

                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });
                //.AddOpenIdConnect(options =>
                //{
                //    options.SignInScheme = "Cookies";
                //    options.Authority = "-your-identity-provider-";
                //    options.RequireHttpsMetadata = true;
                //    options.ClientId = "-your-clientid-";
                //    options.ClientSecret = "-your-client-secret-from-user-secrets-or-keyvault";
                //    options.ResponseType = "code";
                //    options.UsePkce = true;
                //    options.Scope.Add("profile");
                //    options.SaveTokens = true;
                //}); 

            return services;
        }

        public static IServiceCollection AddApplicationJwtAuth(this IServiceCollection services, JwtConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.Issuer,
                        ValidAudience = configuration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key))
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //// Policy-based Role authorization
                //// CategoryController
                //options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                //{
                //    policy.RequireRole(TS.Roles.Admin);
                //});

                //options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                //{
                //    policy.RequireRole(
                //        TS.Roles.Contributor,
                //        TS.Roles.Admin);
                //});

                //options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                //{
                //    policy.RequireRole(
                //        TS.Roles.User,
                //        TS.Roles.Contributor,
                //        TS.Roles.Admin);
                //});


                //// Calim-based authorization
                //// ProductController
                //options.AddPolicy("CaimBasedPolicy", policy =>
                //{
                //    policy.RequireClaim("Product");
                //});


                //// Calim-based authorization using value
                //// ProductController
                //options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Product,
                //        TS.Permissions.Delete.ToString(),
                //        TS.Permissions.Update.ToString());
                //});

                //options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Product,
                //        TS.Permissions.Write.ToString());
                //});

                //options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                //{
                //    policy.RequireClaim(TS.Contoller.Product,
                //        TS.Permissions.Read.ToString());
                //});


                //Policy-based requierment authorization
                //ModuleController
                options.AddPolicy(TS.Policies.FullControlPolicy, policy =>
                {
                    policy.Requirements.Add(new AdminRequirements());
                });

                options.AddPolicy(TS.Policies.ReadAndWritePolicy, policy =>
                {
                    policy.Requirements.Add(new ContributorRequirements());
                });

                options.AddPolicy(TS.Policies.ReadPolicy, policy =>
                {
                    policy.Requirements.Add(new UserRequirements());
                });



                options.AddPolicy(TS.Policies.GenericPolicy, policy =>
                {
                    policy.Requirements.Add(new ConventionBasedRequirements());
                });
            });

            //services.AddSingleton<IAuthorizationHandler, AdminRequirementHandler>();
            //services.AddSingleton<IAuthorizationHandler, ContributorRequirementHandler>();
            //services.AddSingleton<IAuthorizationHandler, UserRequirementHandler>();

            services.AddSingleton<IAuthorizationHandler, GenericRequirmentsHandler>();

            services.AddSingleton<IAuthorizationHandler, ConventionBasedRequirementHandler>();

            return services;
        }

        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {

                var cntx = scope.ServiceProvider.GetRequiredService<UserIdentityContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await cntx.Database.EnsureDeletedAsync();
                if (await cntx.Database.EnsureCreatedAsync())
                {
                    // Creating Role Entities
                    var adminRole = new IdentityRole(TS.Roles.Admin);
                    var contributorRole = new IdentityRole(TS.Roles.Contributor);
                    var userRole = new IdentityRole(TS.Roles.User);

                    // Adding Roles
                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(contributorRole);
                    await roleManager.CreateAsync(userRole);

                    // Creating User Entities
                    var adminUser = new IdentityUser() { UserName = "admin", Email = "admin@EShop.com" };
                    var guest = new IdentityUser() { UserName = "guest", Email = "guest@EShop.com" };

                    // Adding Users with Password
                    await userManager.CreateAsync(adminUser, "Etm@14863");
                    await userManager.CreateAsync(guest, "Etm@guest!2#4%6");

                    // Ading Claims to Users
                    await userManager.AddClaimAsync(adminUser, GetAdminClaims(TS.Contoller.Product));
                    await userManager.AddClaimAsync(guest, GetUserClaims(TS.Contoller.Product));

                    // Adding Roles to Users
                    await userManager.AddToRoleAsync(adminUser, TS.Roles.Admin);
                    await userManager.AddToRoleAsync(guest, TS.Roles.User);

                    //Ading Claims to Roles
                    await roleManager.AddClaimAsync(adminRole, GetAdminClaims(TS.Contoller.Module));
                    await roleManager.AddClaimAsync(contributorRole, GetcontributorClaims(TS.Contoller.Module));
                    await roleManager.AddClaimAsync(userRole, GetUserClaims(TS.Contoller.Module));

                    await roleManager.AddClaimAsync(adminRole, GetAdminClaims(TS.Contoller.Supplier));
                    await roleManager.AddClaimAsync(userRole, GetUserClaims(TS.Contoller.Supplier));

                }
            }
            return app;
        }

        private static Claim GetAdminClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read,
                            TS.Permissions.Write,
                            TS.Permissions.Update,
                            TS.Permissions.Delete
                        ));
        }
        private static Claim GetcontributorClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read,
                            TS.Permissions.Write
                        ));
        }
        private static Claim GetUserClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            TS.Permissions.Read
                        ));
        }
    }
}
