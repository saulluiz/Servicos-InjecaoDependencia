using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

public class MiddlewareConsultaCep
{
    private readonly RequestDelegate next; //guarda a referencia do proximo middleware
    public MiddlewareConsultaCep(RequestDelegate nextMiddleware)
    {
        next = nextMiddleware;
    }
    public MiddlewareConsultaCep()
    {

    }
    public async Task Invoke(HttpContext context)
    {

        if (context.Request.Path.StartsWithSegments("/mw/classe"))
        {

            string[] segmentos = context.Request.Path.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
            string cep = segmentos.Length > 2 ? segmentos[2] : "01001000";
            var objetoCep = await ConsultaCep(cep);
            await TypeBroker.FormatadorEndereco.Formatar(context, objetoCep);

        }


        if (next != null)
        {
            //Quando o endpoint nao leva cumpre o caminho cep e valor, vamos ao proximo middleware. No caso, o proximo confere
            //se o endp[oint tem o caminho especifico e, se nao tiver, vai ao proximo middleware tmb
            await next(context);
        }

    }
    private async Task<jsonCep> ConsultaCep(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json";
        var cliente = new HttpClient();

        cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta Cep");
        var response = await cliente.GetAsync(url);
        var dadosCEP = await response.Content.ReadAsStringAsync();
        dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
        return JsonConvert.DeserializeObject<jsonCep>(dadosCEP);
    }
}