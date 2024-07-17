using API_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Controllers
{
    [Route("/Episode")]
    [ApiController]
    [Authorize]
    public class EpisodesController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;

        public EpisodesController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Episode>>> Get([FromQuery] int page=1, [FromQuery] int pagesize = 20)
        {
            if(page <=0 || pagesize <=0)
            {
                return BadRequest();
            }
            var episodes = await _moviesDbContext.Episodes
                .Skip((page-1) * pagesize).Take(pagesize).ToListAsync();
            return episodes;
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<IEnumerable<Episode>>> GetBySlug(string slug)
        {
            var mov = _moviesDbContext.Movies.Where(x=>x.Slug.Contains(slug)).FirstOrDefault();
            var episodes = _moviesDbContext.Episodes.Where(x=>x.MovieId == mov.Id).ToList();
            return episodes;
        }
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<Episode>> GetByID(int id)
        {
            var episode = _moviesDbContext.Episodes.FirstOrDefault(x => x.Id == id);
            return episode;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Episode>> Create(Episode episode)
        {
            try
            {
                episode.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                episode.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    
                _moviesDbContext.Add(episode);
                
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(episode);
        }
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Episode>> Update(int id, Episode epi)
        {
            var episode = await _moviesDbContext.Episodes.FirstOrDefaultAsync(x => x.Id == id);

            if (episode == null) { return NotFound(); }

            try { 
            episode.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            episode.MovieId = epi.MovieId;
            episode.Server = epi.Server;
            episode.Link = epi.Link;
            episode.Episode1 = epi.Episode1;

             await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(episode);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Episode>> Delete(int id)
        {
            var episode = await _moviesDbContext.Episodes.FirstOrDefaultAsync(x => x.Id == id);
            if(episode==null)
            {
                return NotFound();
            }
            try
            {
                _moviesDbContext.Remove(episode);
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}
