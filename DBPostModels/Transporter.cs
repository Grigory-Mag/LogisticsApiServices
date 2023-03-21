using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Transporter
    {
        public Transporter()
        {
            Vehicles = new HashSet<Vehicle>();
            VehiclesNavigation = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public int? Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Vehicle> VehiclesNavigation { get; set; }
    }
}
