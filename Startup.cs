using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Servicos_InjecaoDependencia
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFormatadorEndereco,EnderecoHtml>();
            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFormatadorEndereco formatador)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseMiddleware<MiddlewareConsultaCep>();

            app.Use(async (context, next) =>
            {

                if (context.Request.Path == "/mw/lambda")
                {
                    await formatador.Formatar(context, await EndpointConsultaCep.ConsultaCep("01001000"));
                }
                else
                {
                    await next();
                }
            });
                app.UseEndpoints(endpoints =>{
                    endpoints.MapEndpoint<EndpointConsultaCep>("/ep/classe/{cep:regex(^\\d{{8}}$)?}");
                    endpoints.MapGet("/ep/lambda/{cep:regex(^\\d{{8}}$)?}",async context=>{
                        string cep= context.Request.RouteValues["cep"] as string ?? "01001000";
                        await formatador.Formatar(context, await EndpointConsultaCep.ConsultaCep(cep));
                    });
                });
            app.Run((context)=>{

               return  context.Response.WriteAsync("Middleware Terminal");
            }
            );

        }
    }
}
