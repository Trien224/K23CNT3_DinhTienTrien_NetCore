using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<DttBook> DttBooks { get; set; } = new List<DttBook>();
}
