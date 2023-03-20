using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Ownership
    {
        public Ownership()
        {
            Requisites = new HashSet<Requisite>();
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ColumnName { get; set; }

        public virtual ICollection<Requisite> Requisites { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
