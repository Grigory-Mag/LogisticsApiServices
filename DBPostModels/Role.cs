using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Requisite> Requisites { get; set; } = new List<Requisite>();
}
