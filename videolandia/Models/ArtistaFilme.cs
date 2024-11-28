using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using videolandia.Models;

namespace videolandia.Models
{
    public class ArtistaFilme
    {
        public int ArtistaId { get; set; }
        public Artista? Artista { get; set; }
        public int FilmeId { get; set; }
        public Filme? Filme { get; set; }

        [Display(Name = "Personagem")]
        public string Personagem { get; set; }

    }
}
