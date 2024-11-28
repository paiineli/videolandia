using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using videolandia.Data;
using videolandia.Models;

namespace videolandia.Controllers
{
    public class FilmesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Filme == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var movies = from m in _context.Filme
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.NomeFilme!.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await movies.ToListAsync());
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .Include(x => x.Artistas)
                .ThenInclude(x => x.Artista)
                .FirstOrDefaultAsync(m => m.Id == id);

            var genero = await _context.Filme
                .Include(x => x.Generos)
                .ThenInclude(x => x.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("Id,NomeFilme,PosterUrl,DtLancamento,Diretor")] Filme filme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filme);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }
            return View(filme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,NomeFilme,PosterUrl,DtLancamento,Diretor")] Filme filme)
        {
            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.Id))
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
            return View(filme);
        }

        public async Task<IActionResult> Deletar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(int id)
        {
            var filme = await _context.Filme.FindAsync(id);
            if (filme != null)
            {
                _context.Filme.Remove(filme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return _context.Filme.Any(e => e.Id == id);
        }
    }
}
