using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class Episode
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string Link { get; set; } = null!;

    public string Episode1 { get; set; } = null!;

    public int? Server { get; set; }

    public string UpdatedAt { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
