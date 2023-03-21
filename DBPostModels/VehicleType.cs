using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class VehicleType
    {
        public VehicleType()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
