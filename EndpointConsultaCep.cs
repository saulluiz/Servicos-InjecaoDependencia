using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

public class EndpointConsultaCep
{   
            public EndpointConsultaCep(){
               
            }


    public  async Task Endpoint(HttpContext context,IFormatadorEndereco formatador)
    {
        string cep = context.Request.RouteValues["cep"] as string ?? "32676554";
        Console.WriteLine(cep);

        var objetoCep = await ConsultaCep(cep);
        if (objetoCep == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else
        {
            await formatador.Formatar(context, objetoCep);

        }


    }
    public  static async Task<jsonCep> ConsultaCep(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json";
        var cliente = new HttpClient();

        cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta Cep");
        var response = await cliente.GetAsync(url);
        var dadosCEP = await response.Content.ReadAsStringAsync();
        dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
        return dadosCEP.Contains("\"error\":") ? null : JsonConvert.DeserializeObject<jsonCep>(dadosCEP);

    }
}