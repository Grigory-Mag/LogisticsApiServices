using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Ownership
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Requisite> Requisites { get; set; } = new List<Requisite>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
