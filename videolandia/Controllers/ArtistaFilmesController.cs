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
	public class ArtistaFilmesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ArtistaFilmesController(ApplicationDbContext context)
		{
			_context = context;
		}
		private bool ArtistaFilmesExists(int artistaId, int filmeId)
		{
			return _context.ArtistaFilme.Any(af => af.ArtistaId == artistaId && af.FilmeId == filmeId);
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.ArtistaFilme.Include(a => a.Artista).Include(a => a.Filme);
			return View(await applicationDbContext.ToListAsync());
		}

		public async Task<IActionResult> Detalhes(int artistaId, int filmeId)
		{
			if (artistaId == 0 || filmeId == 0)
			{
				return NotFound();
			}

			var artistaFilme = await _context.ArtistaFilme
				.Include(af => af.Artista)
				.Include(af => af.Filme)
				.FirstOrDefaultAsync(af => af.ArtistaId == artistaId && af.FilmeId == filmeId);

			if (artistaFilme == null)
			{
				return NotFound();
			}

			return View(artistaFilme);
		}

		public IActionResult Criar()
		{
			ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "NomeArtista");
			ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "NomeFilme");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Criar([Bind("ArtistaId,NomeArtista,FilmeId,NomeFilme,Personagem")] ArtistaFilme artistaFilme)
		{
			if (ModelState.IsValid)
			{
				_context.Add(artistaFilme);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Id", artistaFilme.ArtistaId);
			ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Id", artistaFilme.FilmeId);
			return View(artistaFilme);
		}

		public async Task<IActionResult> Editar(int artistaId, int filmeId)
		{
			if (artistaId == 0 || filmeId == 0)
			{
				return NotFound();
			}

			var artistaFilme = await _context.ArtistaFilme
				.FirstOrDefaultAsync(af => af.ArtistaId == artistaId && af.FilmeId == filmeId);

			if (artistaFilme == null)
			{
				return NotFound();
			}

			ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "NomeArtista", artistaFilme.ArtistaId);
			ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "NomeFilme", artistaFilme.FilmeId);

			return View(artistaFilme);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Editar(int artistaId, int filmeId, [Bind("ArtistaId,FilmeId,Personagem")] ArtistaFilme artistaFilme)
		{
			if (artistaId != artistaFilme.ArtistaId || filmeId != artistaFilme.FilmeId)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(artistaFilme);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ArtistaFilmesExists(artistaFilme.ArtistaId, artistaFilme.FilmeId))
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
			ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome", artistaFilme.ArtistaId);
			ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Titulo", artistaFilme.FilmeId);

			return View(artistaFilme);
		}

		public async Task<IActionResult> Deletar(int artistaId, int filmeId)
		{
			if (artistaId == 0 || filmeId == 0)
			{
				return NotFound();
			}
			var artistaFilme = await _context.ArtistaFilme
				.Include(af => af.Artista)
				.Include(af => af.Filme)
				.FirstOrDefaultAsync(af => af.ArtistaId == artistaId && af.FilmeId == filmeId);

			if (artistaFilme == null)
			{
				return NotFound();
			}
			return View(artistaFilme);
		}

		[HttpPost, ActionName("Deletar")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletarConfirmado(int artistaId, int filmeId)
		{
			var artistaFilme = await _context.ArtistaFilme
				.FirstOrDefaultAsync(af => af.ArtistaId == artistaId && af.FilmeId == filmeId);

			if (artistaFilme == null)
			{
				return NotFound();
			}

			_context.ArtistaFilme.Remove(artistaFilme);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
