using System.ComponentModel.DataAnnotations;

namespace ClienteApiFinal.Dtos
{
    public class ContatoDTO
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Tipo { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string Texto { get; set; } = string.Empty;
    }
}
