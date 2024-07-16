using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class Job
{
    public decimal Id { get; set; }

    public string Queue { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public byte Attempts { get; set; }

    public long? ReservedAt { get; set; }

    public long AvailableAt { get; set; }

    public long CreatedAt { get; set; }
}
