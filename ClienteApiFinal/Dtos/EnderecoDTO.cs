using System.ComponentModel.DataAnnotations;

namespace ClienteApiFinal.Dtos
{
    public class EnderecoDTO
    {
        [Required(AllowEmptyStrings = false), RegularExpression(@"^\d{5}[-]{0,1}\d{3}$")]
        public string Cep { get; set; } = string.Empty;

        public string? Logradouro { get; set; }

        public string? Cidade { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
