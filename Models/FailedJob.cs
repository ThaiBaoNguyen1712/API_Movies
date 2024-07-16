using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class FailedJob
{
    public decimal Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Connection { get; set; } = null!;

    public string Queue { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public string Exception { get; set; } = null!;

    public DateTime FailedAt { get; set; }
}
