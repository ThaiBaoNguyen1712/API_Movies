using API_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Controllers
{
    [Route("/Country")]
    [ApiController]
    [Authorize]
    public class CountriesController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;

        public CountriesController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> Get()
        {
            return await _moviesDbContext.Countries.ToListAsync();
        }

        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<Country>> getByID(int id)
        {
            var country = await _moviesDbContext.Countries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Country>> Create(Country country)
        {
            if(country ==null) { return BadRequest(); }
            try
            {
                _moviesDbContext.Countries.Add(country);
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok(country);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Country>> Update(int id, Country country)
        {
            var coun = await _moviesDbContext.Countries.FirstOrDefaultAsync(x=>x.Id == id);
            if(coun  == null)  { return NotFound(); }

            coun.Title = country.Title;
            coun.Status = country.Status;
            coun.Slug   = country.Slug;
            coun.Description = country.Description;
            coun.Position   = country.Position;

            try
            {
                _moviesDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(coun);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Country>> Delete(int id)
        {
            var country = await _moviesDbContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            { return NotFound(); }
            _moviesDbContext.Countries.Remove(country);
            _moviesDbContext.SaveChanges();
            return NoContent();
        }
    }
}
