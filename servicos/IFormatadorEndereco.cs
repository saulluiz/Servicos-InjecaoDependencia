using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IEndereco
{
    
        
        public string CEP{get;set;}

        public string Logradouro{get;set;}

        public string Complemento{get;set;}

        public string Bairro{get;set;}

        public string Localidade {get;set;}

        public string Estado{get;set;}

}
public interface IFormatadorEndereco
{
    public Task Formatar(HttpContext context, IEndereco endereco);
}

