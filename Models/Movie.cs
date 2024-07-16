using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Thoiluong { get; set; }

    public string NameEng { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Status { get; set; }

    public string Image { get; set; } = null!;

    public string Thuocphim { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? GenreId { get; set; }

    public int CountryId { get; set; }

    public int PhimHot { get; set; }

    public int? Resolution { get; set; }

    public int Phude { get; set; }

    public int? Season { get; set; }

    public string? CreateAt { get; set; }

    public string? UpdateAt { get; set; }

    public string? Year { get; set; }

    public string Tags { get; set; } = null!;

    public int? Topview { get; set; }

    public string? Trailer { get; set; }

    public string Sotap { get; set; } = null!;

    public int? Views { get; set; }

    public string? Actor { get; set; }

    public string? Director { get; set; }

    public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    public virtual ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
