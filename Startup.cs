using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ChatApi.Hubs;
using Microsoft.EntityFrameworkCore;
using ChatApi.Context;

namespace ChatApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var connection = @"Server=(localdb)\mssqllocaldb;Database=ChatProject;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ChatContext>(options => options.UseSqlServer(connection));
            services.AddSignalR();
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {

                routes.MapHub<MessageHub>("message");
            });
            app.UseMvc();
        }
    }
}
