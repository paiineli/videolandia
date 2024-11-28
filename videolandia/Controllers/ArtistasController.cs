using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using videolandia.Data;
using videolandia.Models;

namespace videolandia.Controllers
{
    public class ArtistasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Artista == null)
            {
                return Problem("A entidade 'MvcMovieContext.Movie' é nula.");
            }

            var artistas = from a in _context.Artista
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                artistas = artistas.Where(s => s.NomeArtista!.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await artistas.ToListAsync());
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista
                .Include(x => x.Filmes)
                .ThenInclude(x => x.Filme)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("Id,NomeArtista,DtNascimento,PaisNascimento,ArtistaUrl")] Artista artista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista.FindAsync(id);
            if (artista == null)
            {
                return NotFound();
            }
            return View(artista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,NomeArtista,DtNascimento,PaisNascimento,ArtistaUrl")] Artista artista)
        {
            if (id != artista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistaExiste(artista.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
        }

        public async Task<IActionResult> Deletar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(int id)
        {
            var artista = await _context.Artista.FindAsync(id);
            if (artista != null)
            {
                _context.Artista.Remove(artista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistaExiste(int id)
        {
            return _context.Artista.Any(e => e.Id == id);
        }
    }
}
