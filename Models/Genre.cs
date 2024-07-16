﻿using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Status { get; set; }

    public string Slug { get; set; } = null!;

    public int Position { get; set; }
}
