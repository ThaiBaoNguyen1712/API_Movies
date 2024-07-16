using API_Movies.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Movies.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;

        public EpisodesController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<Episode>> Get()
        {
            var episodes = await _moviesDbContext.Episodes.ToList();
            return episodes;
        }
    }
}
