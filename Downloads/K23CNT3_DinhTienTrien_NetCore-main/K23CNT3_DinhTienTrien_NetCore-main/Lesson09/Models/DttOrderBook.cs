using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttOrderBook
{
    public string OrderId { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public Guid? AccountId { get; set; }

    public string? ReceiveAddress { get; set; }

    public string? ReceivePhone { get; set; }

    public DateTime? OrderReceive { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public virtual DttAccount? Account { get; set; }

    public virtual ICollection<DttOrderDetail> DttOrderDetails { get; set; } = new List<DttOrderDetail>();
}
