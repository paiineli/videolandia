using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using videolandia.Models;

namespace videolandia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artista> Artista { get; set; } = default!;
        public DbSet<Contato> Contato { get; set; } = default!;
        public DbSet<Genero> Genero { get; set; } = default!;
        public DbSet<Filme> Filme { get; set; } = default!;

        public DbSet<ArtistaFilme> ArtistaFilme { get; set; } = default!;
        public DbSet<GeneroFilme> GeneroFilme { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configuração de chave primária composta para ArtistaFilme
            builder.Entity<ArtistaFilme>()
                .HasKey(f => new { f.ArtistaId, f.FilmeId });

            // Relacionamento entre ArtistaFilme e Filme
            builder.Entity<ArtistaFilme>()
                .HasOne(e => e.Filme)
                .WithMany(e => e.Artistas)
                .HasForeignKey(e => e.FilmeId);

            // Relacionamento entre ArtistaFilme e Artista
            builder.Entity<ArtistaFilme>()
                .HasOne(e => e.Artista)
                .WithMany(e => e.Filmes)
                .HasForeignKey(e => e.ArtistaId);

            // Configuração de chave primária composta para GeneroFilme
            builder.Entity<GeneroFilme>()
                .HasKey(f => new { f.GeneroId, f.FilmeId });

            // Relacionamento entre GeneroFilme e Filme
            builder.Entity<GeneroFilme>()
                .HasOne(e => e.Filme)
                .WithMany(e => e.Generos)
                .HasForeignKey(e => e.FilmeId);

            // Relacionamento entre GeneroFilme e Genero
            builder.Entity<GeneroFilme>()
                .HasOne(e => e.Genero)
                .WithMany(e => e.Filmes)
                .HasForeignKey(e => e.GeneroId);

            base.OnModelCreating(builder);
        }
    }
}
