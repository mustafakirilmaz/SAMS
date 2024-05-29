using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SAMS.Common;
using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.DataAccess;
using SAMS.ExceptionHandling;
using SAMS.Infrastructure.Attributes;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.WebToken;
using SAMS.Server.Hangfire;
using SAMS.Server.ServiceContracts;
using SAMS.Server.Services;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace SAMS.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IFilesBusinessService, FileBusinessService>();
            services.AddScoped<IAccountBusinessService, AccountBusinessService>();
            services.AddScoped<IUserBusinessService, UserBusinessService>();
            services.AddScoped<ICommonBusinessService, CommonBusinessService>();
            services.AddScoped<ISiteBusinessService, SiteBusinessService>();
            services.AddScoped<IBuildingBusinessService, BuildingBusinessService>();
            services.AddScoped<IUnitBusinessService, UnitBusinessService>();
            services.AddScoped<IBusinessProjectBusinessService, BusinessProjectBusinessService>();
            services.AddScoped<IEqualExpenseBusinessService, EqualExpenseBusinessService>();
            services.AddScoped<IProportionalExpenseBusinessService, ProportionalExpenseBusinessService>();
            services.AddScoped<IFixtureExpenseBusinessService, FixtureExpenseBusinessService>();
            services.AddScoped<IExpenseTypeBusinessService, ExpenseTypeBusinessService>();
            services.AddScoped<IExpenseBusinessService, ExpenseBusinessService>();
            #endregion

            #region Other
            #region Swagger Dökümantasyon
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Site ve Apartman Yönetim Sistemi Web Servis Dokümaný", Version = "v1" });
                option.OperationFilter<OpenApiParameterIgnoreFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);

                #region Swagger Authorization
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Geçerli bir token giriniz",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                #endregion
                });
                option.SchemaFilter<EnumTypesSchemaFilter>(xmlPath);
            });
            #endregion

            #region Token çalýnmasýna karþýlýk ip adres doðrulamasý
            services.AddSingleton<IAuthorizationHandler, IpAddressCheckHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SameIpPolicy", policy => policy.Requirements.Add(new IpAddressCheckRequirement { IpClaimRequired = true }));
            });
            #endregion

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            services.AddCors();
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Culture = new CultureInfo("tr-TR");
            });
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.Filters.Add(new AuthorizeFilter("SameIpPolicy"));
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Keys.JwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<SAMSDbContext>(options => options.UseNpgsql(AppSettingsHelper.Current.AppConnection))
                    .AddUnitOfWork<SAMSDbContext>()
                    .AddHangfire(options => options.UsePostgreSqlStorage(AppSettingsHelper.Current.AppConnection));

            services.UseOneTransactionPerHttpCall();

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
                options.MultipartBoundaryLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            #region Validation Filter Attribute
            services.AddScoped<ValidationFilterAttribute>();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            #endregion

            services.AddRazorPages();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //Postgresql tarihlerde hata veriyordu bu yüzden eklendi.
            //https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            #region Default Configuration
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureCustomExceptionMiddleware();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true));
            app.UseMvc(routes => { routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}", defaults: new { controller = "Home", action = "Index" }); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            #endregion

            #region Swagger Configuration
            app.UseSwagger();
            app.UseSwaggerUI();
            #endregion

            #region SPA Configuration
            app.UseRouting();
            #endregion

            #region File Upload Configuration
            var pathName = Path.Combine(Directory.GetCurrentDirectory(), AppSettingsHelper.Current.FileUploadPathName);
            if (!Directory.Exists(pathName))
            {
                Directory.CreateDirectory(pathName);
            }
            app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(pathName), RequestPath = "/" + AppSettingsHelper.Current.FileUploadPathName });
            AppSettingsHelper.Current.FileUploadPathName = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault() + "/" + AppSettingsHelper.Current.FileUploadPathName + "/{0}/{1}";
            #endregion

            #region Hangfire Jobs Configuration
            GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(serviceProvider));
            app.UseHangfireDashboard(pathMatch: "/jobs", options: new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[] { new HangfireDashboardJwtAuthorizationFilter(RoleNames.Admin) },
                AppPath = AppSettingsHelper.Current.CurrentSiteUrl
            });
            app.UseHangfireServer();
            HangfireJobManager.RegisterJobs();
            #endregion
        }
    }
}
