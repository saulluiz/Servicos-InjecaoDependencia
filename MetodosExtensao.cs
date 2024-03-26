using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class MetodosExtensao
    {
        public static void MapEndpoint<T>(this IEndpointRouteBuilder app,
                                          string caminho, string nomeMetodo = "Endpoint")
        {
            MethodInfo mi = typeof(T).GetMethod(nomeMetodo);
            if (mi == null || mi.ReturnType != typeof(Task))
            {
                throw new System.Exception("Metodo não é compatível.");
            }

           T instanciaEndpoint =
                ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            ParameterInfo[] parametros = mi.GetParameters();
            app.MapGet(caminho,context =>(Task)mi.Invoke(instanciaEndpoint,parametros.Select(p=>p.ParameterType == typeof(HttpContext) ? context : context.RequestServices.GetService(p.ParameterType)).ToArray()));
        }
    }
}
