using API_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Controllers
{
    [Route("/LinkMovie")]
    [ApiController]
    [Authorize]
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

        [HttpGet("by-id/{id}")]
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
        [HttpPost("Create")]
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
        [HttpPut("Update/{id}")]
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
        [HttpDelete("Delete/{id}")]
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
