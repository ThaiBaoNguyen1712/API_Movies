using API_Movies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Controllers
{
    [Route("/LinkMovie")]
    [ApiController]
    public class LinkMovieController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;
        public LinkMovieController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Linkmovie>>> Get()
        {
            return await _moviesDbContext.Linkmovies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Linkmovie>> getByID(int id)
        {
            var link = await _moviesDbContext.Linkmovies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }
        [HttpPost]
        public async Task<ActionResult<Linkmovie>> Create(Linkmovie linkmovie)
        {
            try
            {
                _moviesDbContext.Add(linkmovie);
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(linkmovie);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Linkmovie>> Update(int id,Linkmovie linkmovie)
        {
            var link = await _moviesDbContext.Linkmovies.FirstOrDefaultAsync(x => x.Id == id);
            if(link == null) { return NotFound(); }

            link.Title = linkmovie.Title;
            link.Status = linkmovie.Status;
            link.Description = linkmovie.Description;

            try
            {
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(link);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Linkmovie>> Delete(int id)
        {
            var link = await _moviesDbContext.Linkmovies.FirstOrDefaultAsync(x => x.Id == id);
            if(link == null) { 
            return NotFound();}
            _moviesDbContext.Linkmovies.Remove(link);
            _moviesDbContext.SaveChanges();
            return NoContent();
        }
    }
}
