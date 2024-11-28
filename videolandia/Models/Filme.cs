using System.ComponentModel.DataAnnotations;
using videolandia.Models;

namespace videolandia.Models
{
    public class Filme
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Filme")]
        [Required]
        public string NomeFilme { get; set; }

        [Display(Name = "Pôster")]
        [Required]
        [DataType(DataType.Url)]
        public string PosterUrl { get; set; }

        [Display(Name = "Data de Lançamento")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DtLancamento { get; set; }

        [Display(Name = "Diretor")]
        public string Diretor { get; set; }

        public List<ArtistaFilme> Artistas { get; } = new List<ArtistaFilme>();
        public List<GeneroFilme> Generos { get; } = new List<GeneroFilme>();
    }
}
