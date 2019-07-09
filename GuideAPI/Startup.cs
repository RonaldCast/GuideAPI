using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideAPI.Persistens;
using GuideAPI.Services.ColorPaletteService;
using GuideAPI.Services.StyleSheetService;
using GuideAPI.Services.TypepographyService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace GuideAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Guide style API", Version = "V1" });
            });

            var connection = Configuration.GetConnectionString("dev");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IStyleSheetService, StyleSheetService>();
            services.AddScoped<IColorPaletteService, ColorPaletteService>();
            services.AddScoped<ITypepographyService, TypepographyService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guide style API v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
