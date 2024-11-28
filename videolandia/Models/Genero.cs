using System.ComponentModel.DataAnnotations;
using videolandia.Models;

namespace videolandia.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Display(Name = "Gênero")]
        [Required]
        public string NomeGenero { get; set; }

        public List<GeneroFilme> Filmes { get; } = new List<GeneroFilme>();
    }
}
