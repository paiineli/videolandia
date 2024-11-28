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
    public class GeneroFilmesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GeneroFilmesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool GeneroFilmeExists(int generoId, int filmeId)
        {
            return _context.GeneroFilme.Any(gf => gf.GeneroId == generoId && gf.FilmeId == filmeId);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GeneroFilme.Include(g => g.Filme).Include(g => g.Genero);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Detalhes(int generoId, int filmeId)
        {
            if (generoId == 0 || filmeId == 0)
            {
                return NotFound();
            }

            var generoFilme = await _context.GeneroFilme
                .Include(gf => gf.Genero)
                .Include(gf => gf.Filme)
                .FirstOrDefaultAsync(gf => gf.GeneroId == generoId && gf.FilmeId == filmeId);

            if (generoFilme == null)
            {
                return NotFound();
            }

            return View(generoFilme);
        }

        public IActionResult Criar()
        {
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "NomeFilme");
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "NomeGenero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("FilmeId,GeneroId")] GeneroFilme generoFilme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(generoFilme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Id", generoFilme.FilmeId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Id", generoFilme.GeneroId);
            return View(generoFilme);
        }

        public async Task<IActionResult> Editar(int generoId, int filmeId)
        {
            if (generoId == 0 || filmeId == 0)
            {
                return NotFound();
            }

            var generoFilme = await _context.GeneroFilme
                .FirstOrDefaultAsync(gf => gf.GeneroId == generoId && gf.FilmeId == filmeId);

            if (generoFilme == null)
            {
                return NotFound();
            }

            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "NomeGenero", generoFilme.GeneroId);
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "NomeFilme", generoFilme.FilmeId);

            return View(generoFilme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int generoId, int filmeId, [Bind("GeneroId,FilmeId,OutroCampo1,OutroCampo2")] GeneroFilme generoFilme)
        {
            if (generoId != generoFilme.GeneroId || filmeId != generoFilme.FilmeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(generoFilme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroFilmeExists(generoFilme.GeneroId, generoFilme.FilmeId))
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
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "NomeGenero", generoFilme.GeneroId);
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "NomeFilme", generoFilme.FilmeId);
            return View(generoFilme);
        }

        public async Task<IActionResult> Deletar(int generoId, int filmeId)
        {
            if (generoId == 0 || filmeId == 0)
            {
                return NotFound();
            }

            var generoFilme = await _context.GeneroFilme
                .Include(gf => gf.Genero)
                .Include(gf => gf.Filme)
                .FirstOrDefaultAsync(gf => gf.GeneroId == generoId && gf.FilmeId == filmeId);

            if (generoFilme == null)
            {
                return NotFound();
            }

            return View(generoFilme);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(int generoId, int filmeId)
        {
            var generoFilme = await _context.GeneroFilme
                .FirstOrDefaultAsync(gf => gf.GeneroId == generoId && gf.FilmeId == filmeId);

            if (generoFilme == null)
            {
                return NotFound();
            }

            _context.GeneroFilme.Remove(generoFilme);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
