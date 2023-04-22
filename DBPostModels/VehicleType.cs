using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class VehicleType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
