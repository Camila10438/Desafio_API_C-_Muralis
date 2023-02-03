using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteApiFinal.Models
{
    [PrimaryKey(nameof(Id))]
    public class Contato
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Tipo { get; set; }

        public string? Texto { get; set; }
    }
}
