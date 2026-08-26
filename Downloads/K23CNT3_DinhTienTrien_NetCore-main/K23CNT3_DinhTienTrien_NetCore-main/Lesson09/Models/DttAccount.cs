using System;
using System.Collections.Generic;

namespace Lesson09.Models;

public partial class DttAccount
{
    public Guid AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Picture { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DttOrderBook> DttOrderBooks { get; set; } = new List<DttOrderBook>();
}
