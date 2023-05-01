using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class RouteAction
{
    public string Action { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
