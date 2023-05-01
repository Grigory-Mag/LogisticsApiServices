using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Route
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public int Action { get; set; }

    public DateTime ActionDate { get; set; }

    public virtual RouteAction ActionNavigation { get; set; } = null!;

    public virtual ICollection<Request> IdRequests { get; set; } = new List<Request>();
}
