using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttPublisher
{
    public int PublisherId { get; set; }

    public string? PublisherName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<DttBook> DttBooks { get; set; } = new List<DttBook>();
}
