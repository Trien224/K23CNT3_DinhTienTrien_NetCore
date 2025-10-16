using System;
using System.Collections.Generic;

namespace Dtt_2310900107_de05.Models;

public partial class DttTask
{
    public int DttTaskId { get; set; }

    public string DttTaskName { get; set; } = null!;

    public string? DttTaskLevel { get; set; }

    public DateOnly? DttStartDate { get; set; }

    public bool DttTaskStatus { get; set; }

}
