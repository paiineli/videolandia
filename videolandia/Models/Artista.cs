using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;
using videolandia.Models;

namespace videolandia.Models
{
    public class Artista
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Artista")]
        [Required]
        public string NomeArtista { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DtNascimento { get; set; }

        [Display(Name = "País de Nascimento")]
        [Required]
        public string PaisNascimento { get; set; }

        public string ArtistaUrl { get; set; }
        public List<ArtistaFilme> Filmes { get; } = [];
    }
}
