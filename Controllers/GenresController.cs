using API_Movies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Controllers
{
    [Route("/Genre")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;
        public GenresController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> Get()
        {
            return await _moviesDbContext.Genres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> getByID(int id)
        {
            var genre = await _moviesDbContext.Genres
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }


        [HttpPost]
        public async Task<ActionResult<Genre>> Create(Genre genre)
        {
            _moviesDbContext.Add(genre);
            try
            {
                await _moviesDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(genre);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Genre>> Update(int id, Genre genre)
        {
            var gen = await _moviesDbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (gen == null)
            { return NotFound(); }

            gen.Slug = genre.Slug;
            gen.Title = genre.Title;
            gen.Description = genre.Description;
            gen.Position = genre.Position;
            gen.Status = genre.Status;

            try
            {
               await _moviesDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(gen);

            
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteGenre(int id)
        {
            var genre = await _moviesDbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if(genre==null)
            {
                return NotFound();
            }
            _moviesDbContext.Genres.Remove(genre);
            _moviesDbContext.SaveChanges();
            return NoContent();
        }

    }
}
