namespace ClienteApiFinal.Models
{
    public class Endereco
    {
        public string Cep { get; set; } = String.Empty;

        public string? Logradouro { get; set; }

        public string? Cidade { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
