using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Constraint
{
    public int Id { get; set; }

    public string? Desc { get; set; }

    public virtual ICollection<CargoConstraint> CargoConstraints { get; set; } = new List<CargoConstraint>();
}
