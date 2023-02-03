using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteApiFinal.Models
{
    [PrimaryKey(nameof(Id)), Index(nameof(Nome))]
    public class Cliente
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Nome { get; set; }

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;

        public List<Contato>? Contatos { get; set; }

        [Required]
        public Endereco Endereco { get; set; } = new Endereco { };
    }
}
