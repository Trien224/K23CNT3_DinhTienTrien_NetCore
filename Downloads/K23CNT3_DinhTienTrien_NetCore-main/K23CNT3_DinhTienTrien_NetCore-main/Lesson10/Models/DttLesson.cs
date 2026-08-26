using System;
using System.Collections.Generic;

namespace Lesson10.Models;

public partial class DttLesson
{
    public int DttId { get; set; }

    public string? DttName { get; set; }

    public string? DttImage { get; set; }

    public string? DttContent { get; set; }

    public bool DttStatus { get; set; }  
}
