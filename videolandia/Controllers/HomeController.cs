using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using videolandia.Models;
using videolandia.Data;

namespace videolandia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Método Index com consulta assíncrona
        public async Task<IActionResult> Index()
        {
            var filmes = await _context.Filme
                .OrderByDescending(f => f.Id)
                .Take(10)
                .ToListAsync();

            return View(filmes);
        }

        // Método Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Método Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
