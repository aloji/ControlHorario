using ControlHorario.Api.Auth;
using ControlHorario.Api.ModelBinder;
using ControlHorario.Application.Options;
using ControlHorario.AzureTable.DataAccess.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

namespace ControlHorario.Api
{
    public class Startup
    {
        const string SwaggerName = "ControlHorario.Api";
        const string SwaggerVersion = "v1";
        const string SwaggerAuthName = "Custom Scheme";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureTableOptions>(
                this.Configuration.GetSection("AzureTableOptions"));

            services.Configure<FaceOptions>(
                this.Configuration.GetSection("FaceOptions"));

            services
                .AddControlHorarioApi()
                .AddControlHorarioApp()
                .AddControlHorarioDomain()
                .AddControlHorarioAzureTable();

            services.AddMvc(options => {
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                })
                .AddJsonOptions(options => {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info {
                    Title = SwaggerName, Version = SwaggerVersion
                });
                c.AddSecurityDefinition(SwaggerAuthName,
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter valid scheme and token",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { SwaggerAuthName, Enumerable.Empty<string>() },
                });
            });

            var authSchemeOptions = this.Configuration.GetSection("AuthSchemeOptions")
               .Get<AuthSchemeOptions>();

            services.AddAuthKeyScheme(authSchemeOptions);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.Admin, policy =>
                {
                    policy.AddAuthenticationSchemes(authSchemeOptions.Scheme);
                    policy.RequireRole(authSchemeOptions.RoleValue);
                });
            });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"/swagger/{SwaggerVersion}/swagger.json",
                    $"{SwaggerName} {SwaggerVersion}");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
