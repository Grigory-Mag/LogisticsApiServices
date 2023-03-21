using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
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

        public virtual ICollection<Requisite> Requisites { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
