using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class CargoType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
}
