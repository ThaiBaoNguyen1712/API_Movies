using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class MovieCategory
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int CategoryId { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
