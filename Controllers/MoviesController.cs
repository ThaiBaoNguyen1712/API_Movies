using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;


namespace API_Movies.Controllers
{
    [Route("/Phim")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly MoviesDbContext _moviesDbContext;

        public MoviesController(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _moviesDbContext.Movies.Take(20).ToListAsync();
        }
   
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<Movie>> GetMovie_ByID(int id)
        {
            var movie = await _moviesDbContext.Movies
                .Where(m => m.Id==id)
                .Include(m => m.Episodes)
                .Include(m => m.MovieCategories)
                .Include(m => m.MovieGenres)
                .Select(m => new
                {
                    // Các thuộc tính của Movie
                    m.Id,
                    m.Title,
                    m.Thoiluong,
                    m.NameEng,
                    m.Slug,
                    m.Description,
                    m.Status,
                    m.Image,
                    m.Thuocphim,
                    m.CategoryId,
                    m.GenreId,
                    m.CountryId,
                    m.PhimHot,
                    m.Resolution,
                    m.Phude,
                    m.Season,
                    m.CreateAt,
                    m.UpdateAt,
                    m.Year,
                    m.Tags,
                    m.Topview,
                    m.Trailer,
                    m.Sotap,
                    m.Views,
                    m.Actor,
                    m.Director,

                    // Các thuộc tính từ Episodes
                    Episodes = m.Episodes.Select(e => new
                    {
                        e.Episode1,
                        //Link = GetUrlFromLink(e.Link)
                        e.Link
                    }).ToList(),

                    // Các thuộc tính từ MovieCategories
                    Danh_Muc = m.MovieCategories.Select(c => new
                    {
                        CategoryTitle = _moviesDbContext.Categories
                                        .Where(x => x.Id == c.CategoryId).Select(x => x.Title)
                                        .FirstOrDefault()
                    }).ToList(),

                    // Các thuộc tính từ MovieGenres
                    The_Loai = m.MovieGenres.Select(c => new
                    {
                        GenreTitle = _moviesDbContext.Genres
                                .Where(g => g.Id == c.GenreId)
                                .Select(g => g.Title)
                                .FirstOrDefault()

                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<Movie>> GetMovie_BySlug(string slug)
        {
            var movie = await _moviesDbContext.Movies
                .Where(m => m.Slug.Contains(slug))
                .Include(m => m.Episodes)
                .Include(m => m.MovieCategories)
                .Include(m => m.MovieGenres)
                .Select(m => new
                {
                    // Các thuộc tính của Movie
                    m.Id,
                    m.Title,
                    m.Thoiluong,
                    m.NameEng,
                    m.Slug,
                    m.Description,
                    m.Status,
                    m.Image,
                    m.Thuocphim,
                    m.CategoryId,
                    m.GenreId,
                    m.CountryId,
                    m.PhimHot,
                    m.Resolution,
                    m.Phude,
                    m.Season,
                    m.CreateAt,
                    m.UpdateAt,
                    m.Year,
                    m.Tags,
                    m.Topview,
                    m.Trailer,
                    m.Sotap,
                    m.Views,
                    m.Actor,
                    m.Director,

                    // Các thuộc tính từ Episodes
                    Episodes = m.Episodes.Select(e => new
                    {
                        e.Episode1,
                        //Link = GetUrlFromLink(e.Link)
                        e.Link
                    }).ToList(),

                    // Các thuộc tính từ MovieCategories
                    Danh_Muc = m.MovieCategories.Select(c => new
                    {
                        CategoryTitle = _moviesDbContext.Categories
                                        .Where(x=>x.Id == c.CategoryId).Select(x=>x.Title)
                                        .FirstOrDefault()
                    }).ToList(),

                    // Các thuộc tính từ MovieGenres
                    The_Loai = m.MovieGenres.Select(c => new
                    {
                        GenreTitle = _moviesDbContext.Genres
                                .Where(g => g.Id == c.GenreId)
                                .Select(g => g.Title)
                                .FirstOrDefault()

                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        [Route("private_add")]
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            try
            {
                movie.CreateAt = Convert.ToString(DateTime.Now);
                _moviesDbContext.Movies.Add(movie);

                foreach(var cate in movie.MovieCategories)
                {
                    cate.MovieId= movie.Id;
                    _moviesDbContext.MovieCategories.Add(cate);
                }
                foreach(var genre in movie.MovieGenres)
                {
                    genre.MovieId = movie.Id;
                    _moviesDbContext.MovieGenres.Add(genre);
                }
                await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
            // Add the new movie to the database
          

            // Return the created movie
            return CreatedAtAction(nameof(GetMovie_ByID), new { id = movie.Id }, movie);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Update(int id,Movie movie)
        {
            var mov =  await _moviesDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if(mov == null)
            {
                return NotFound();
            }
            try { 
            mov.Title = movie.Title;
            mov.Description = movie.Description;
            mov.Thoiluong = movie.Thoiluong;
            mov.NameEng = movie.NameEng;
            mov.Phude = movie.Phude;
            mov.Year = movie.Year;
            mov.Resolution = movie.Resolution;
            mov.PhimHot = movie.PhimHot;
            mov.Status = movie.Status;
            mov.Director = movie.Director;
            mov.Actor = movie.Actor;
            mov.Image = movie.Image;
            mov.Views = movie.Views;
            mov.Topview = movie.Topview;
            mov.Tags = movie.Tags;
            mov.Sotap = movie.Sotap;
            mov.Trailer= movie.Trailer;
            mov.UpdateAt = Convert.ToString(DateTime.Now);
            mov.CountryId = movie.CountryId;

            foreach(var cate in movie.MovieCategories)
            {
                var move = await _moviesDbContext.MovieCategories.FirstOrDefaultAsync(x => x.MovieId == mov.Id);
                if (move != null)
                {
                    move.MovieId = mov.Id;
                    move.CategoryId = cate.Id;
                }
                else
                {
                    cate.MovieId = mov.Id;
                    _moviesDbContext.MovieCategories.Add(cate);
                }
            }
            foreach(var genre in movie.MovieGenres)
            {
                var move = await _moviesDbContext.MovieGenres.FirstOrDefaultAsync(x => x.MovieId == mov.Id);
                if(move !=null)
                {
                    genre.MovieId = mov.Id;
                    genre.GenreId = genre.Id;
                }
                else
                {
                    genre.MovieId = mov.Id;
                    _moviesDbContext.MovieGenres.Add(genre);
                }
            }
            await _moviesDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(mov);
        }
        [Authorize]
        [Route("private_remove/{id}")]
        [HttpDelete]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _moviesDbContext.Movies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if(movie== null)
            {
                return NotFound();
            }
            _moviesDbContext.Remove(movie);
            await _moviesDbContext.SaveChangesAsync();

            return NoContent(); // HTTP 204
        }

    }
}
