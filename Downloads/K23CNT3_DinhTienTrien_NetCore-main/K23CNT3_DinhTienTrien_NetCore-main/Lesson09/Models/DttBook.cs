using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttBook
{
    public string BookId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public int? Release { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public string? Picture { get; set; }

    public int? PublisherId { get; set; }

    public int? CategoryId { get; set; }

    public virtual DttCategory? Category { get; set; }

    public virtual ICollection<DttOrderDetail> DttOrderDetails { get; set; } = new List<DttOrderDetail>();

    public virtual DttPublisher? Publisher { get; set; }
}
