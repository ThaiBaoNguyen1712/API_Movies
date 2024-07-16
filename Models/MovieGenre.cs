using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class MovieGenre
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int GenreId { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
