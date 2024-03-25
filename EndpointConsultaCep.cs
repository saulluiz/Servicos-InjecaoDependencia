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
  

    public static async Task Endpoint(HttpContext context)
    {


        string cep = context.Request.RouteValues["cep"] as string ?? "32676554";
        Console.WriteLine(cep);




        var objetoCep = await ConsultaCep(cep);
        if(objetoCep ==null){
            context.Response.StatusCode=StatusCodes.Status404NotFound;
        }
        else{

        context.Response.ContentType = "text/html; charset=utf-8";

        StringBuilder html = new StringBuilder();
        html.Append($"<h3>CEP {objetoCep.CEP}</p>");
        html.Append($"<p>Logradouro: {objetoCep.Logradouro}</p>");
        html.Append($"<p>Bairro: {objetoCep.Bairro}</p>");
        html.Append($"<p>Cidade/UF: {objetoCep.Localidade}/{objetoCep.Estado}</p>");
        string localidade = HttpUtility.UrlEncode($"{objetoCep.Localidade}-{objetoCep.Estado}");
        LinkGenerator geradorLink =  context.RequestServices.GetService<LinkGenerator>();
        string url = geradorLink.GetPathByRouteValues(context,"consultapop",new {local=localidade});

        await context.Response.WriteAsync(html.ToString());

        } 


    }
    public static async Task<jsonCep> ConsultaCep(string cep)
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