public class TypeBroker
{
    private static IFormatadorEndereco instanciaCompartilhada=new EnderecoTextual();
    public static IFormatadorEndereco FormatadorEndereco => instanciaCompartilhada;
}