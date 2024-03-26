public class TypeBroker
{
    private static IFormatadorEndereco instanciaCompartilhada=new EnderecoHtml();
    public static IFormatadorEndereco FormatadorEndereco => instanciaCompartilhada;
}