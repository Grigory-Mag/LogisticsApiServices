using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Cargo
    {
        public Cargo()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int? Type { get; set; }
        public int? Constraints { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }

        public virtual CargoType? TypeNavigation { get; set; }
        public virtual CargoConstraint CargoConstraint { get; set; } = null!;
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
