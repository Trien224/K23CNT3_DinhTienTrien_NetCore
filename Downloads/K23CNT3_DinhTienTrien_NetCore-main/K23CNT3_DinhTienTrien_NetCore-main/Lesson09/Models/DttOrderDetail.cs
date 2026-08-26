using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttOrderDetail
{
    public int OrderDetailId { get; set; }

    public string? OrderId { get; set; }

    public string? BookId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public int? TotalMoney { get; set; }

    public virtual DttBook? Book { get; set; }

    public virtual DttOrderBook? Order { get; set; }
}
