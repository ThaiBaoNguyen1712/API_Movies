using API_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace API_Movies.Controllers
{
    [Route("/Category")]
    [ApiController]
    [Authorize]

    public class CategoriesController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;

        public CategoriesController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _moviesDbContext.Categories.ToListAsync();
            return categories;
        }
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<Category>> getByID(int id)
        {
            var cate = await _moviesDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (cate == null)
            {
                return NotFound();
            }
            return cate;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            try
            {
                _moviesDbContext.Categories.Add(category);
                await _moviesDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(category);
        }
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Category>> Update(int id, Category category)
        {
            var cate = await _moviesDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(cate==null)
            {
            return NotFound();

            }

            cate.Slug = category.Slug;
            cate.Title = category.Title;
            cate.Status= category.Status;
            cate.Description = category.Description;
            cate.AppearNav=category.AppearNav;
            cate.Position=category.Position;
            try
            {
                await _moviesDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_moviesDbContext.Categories.Any(e => e.Id == id))
            {
                return NotFound();
            }

            return Ok(cate);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            var cate = await _moviesDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(cate == null)
            { return NotFound();}
            _moviesDbContext.Categories.Remove(cate);
            _moviesDbContext.SaveChanges();
            return NoContent();
        }
    }
}
