namespace videolandia.Models
{
    public class GeneroFilme
    {
        public int FilmeId { get; set; }
        public Filme? Filme { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
    }
}
