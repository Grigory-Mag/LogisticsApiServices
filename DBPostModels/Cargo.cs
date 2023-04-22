using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Cargo
{
    public int Id { get; set; }

    public int? Type { get; set; }

    public double? Weight { get; set; }

    public double? Volume { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<CargoConstraint> CargoConstraints { get; set; } = new List<CargoConstraint>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual CargoType? TypeNavigation { get; set; }
}
