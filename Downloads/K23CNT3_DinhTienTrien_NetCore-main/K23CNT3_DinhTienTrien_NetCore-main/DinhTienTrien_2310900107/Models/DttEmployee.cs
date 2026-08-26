using System;
using System.Collections.Generic;

namespace DinhTienTrien_2310900107.Models;

public partial class DttEmployee
{
    public int DttEmpId { get; set; }

    public string? DttEmpName { get; set; }

    public string? DttEmpLevel { get; set; }

    public DateOnly? DttStartDate { get; set; }

    public bool? DttEmpStatus { get; set; }
}
