using System.ComponentModel.DataAnnotations;

namespace ClienteApiFinal.Dtos
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; } = string.Empty;

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;

        public List<ContatoDTO>? Contatos { get; set; }

        public int EnderecoId { get; set; }

        public EnderecoDTO? Endereco { get; set; }
    }
}
