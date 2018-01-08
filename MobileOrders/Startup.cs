using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobileOrders.Models;
using MobileOrders.Services;
using MobileOrders.Middlewares;

namespace MobileOrders
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
			services.AddDbContext<OrdersDbContext>(options =>
	  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddLogRequest();
			services.AddCors();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				   .AddJwtBearer(options =>
				   {
					   options.RequireHttpsMetadata = false;
					   options.TokenValidationParameters = new TokenValidationParameters
					   {
						   // укзывает, будет ли валидироваться издатель при валидации токена
						   ValidateIssuer = true,
						   // строка, представляющая издателя
						   ValidIssuer = AuthOptions.ISSUER,

						   // будет ли валидироваться потребитель токена
						   ValidateAudience = true,
						   // установка потребителя токена
						   ValidAudience = AuthOptions.AUDIENCE,
						   // будет ли валидироваться время существования
						   ValidateLifetime = true,

						   // установка ключа безопасности
						   IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
						   // валидация ключа безопасности
						   ValidateIssuerSigningKey = true,
					   };
				   });
			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			app.UseCors(builder =>
						builder.AllowAnyOrigin().AllowAnyHeader()
						);
			//app.UseMiddleware<LogRequestMiddleware>();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc();
        }
    }
}
