using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Transporter
    {
        public Transporter()
        {
            IdVehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Vehicle> IdVehicles { get; set; }
    }
}
