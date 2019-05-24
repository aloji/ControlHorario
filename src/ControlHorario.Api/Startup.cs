﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlHorario.AzureTable.DataAccess.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace ControlHorario.Api
{
    public class Startup
    {
        const string SwaggerName = "ControlHorario.Api";
        const string SwaggerVersion = "v1";
        
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

            services
                .AddControlHorarioApi()
                .AddControlHorarioApp()
                .AddControlHorarioAzureTable();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info {
                    Title = SwaggerName, Version = SwaggerVersion
                });
            });
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