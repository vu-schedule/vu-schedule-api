using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VuScheduleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var client = new WebClient("https://mif.vu.lt/timetable", "ji3fs9qivs0lrq77xa68ao2bny410i53");
            services.AddSingleton(new SubjectsService(client));
            services.AddSingleton(new StudyService(client));
            services.AddSingleton(new CalendarService(client));

            services.AddCors();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin());

            app.UseMvc();
        }
    }
}
