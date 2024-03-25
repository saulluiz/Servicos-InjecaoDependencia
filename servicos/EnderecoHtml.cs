using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class EnderecoHtml : IFormatadorEndereco
{
    private int contadorDeUso = 0;

    public async Task Formatar(HttpContext context, IEndereco endereco)
    {
        StringBuilder conteudo = new StringBuilder();
        conteudo.Append($"<h3>Dados de CEP {endereco.CEP}</h3>");
        conteudo.Append($"<p>Logradouro: {endereco.Logradouro}</p>");
        conteudo.Append($"<p>Complemento: {endereco.Complemento}</p>");
        conteudo.Append($"<p>Bairro: {endereco.Bairro}</p>");
        conteudo.Append($"<p>Cidade/UF: {endereco.Localidade}/{endereco.Estado}</p>");
        conteudo.Append($"<small>Formatador usado {++contadorDeUso} vez(es).</small>");

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(conteudo.ToString());
    }
}
