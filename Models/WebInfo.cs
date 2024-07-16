using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class WebInfo
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Logo { get; set; } = null!;
}
