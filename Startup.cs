using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailBank.App;
using MailBank.Formats;
using MailBank.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MailBank.Filters.StatusToBody;
using MailBank.Filters.Validation;

namespace MailBank
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
            services.AddControllers();

            services.AddAuthentication("ClientId")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationByClientIdHandler>("ClientId", opt => {
            });


            services.AddTransient<IValidator, NewProductValidator>();
            services.AddTransient<IDescriptionValidator, DescriptionLenghtValidtor>();
            services.AddTransient<INameValidator, NameLengthValidator>();

            services.AddTransient<AuthorizationPolicy>();
            services.AddTransient<HttpStatusToBodyFormatFilter>();
            services.AddTransient<ValidationFilter>();
            services.AddMvc((opt) =>
            {
                opt.InputFormatters.Add(new ProductInputFormatter());
                opt.Filters.AddService<ValidationFilter>();
                opt.Filters.Add( new HttpStatusToBodyFormatFilter(
                        new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
                    ));
            });

            services.AddTransient<IProductsRepository>((p) => new JsonFileProductRepository(
                () => Configuration["DbFilePath"]
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
