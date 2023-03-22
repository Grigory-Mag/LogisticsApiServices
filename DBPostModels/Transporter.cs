using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Transporter
    {
        public Transporter()
        {
            TransportersVehicles = new HashSet<TransportersVehicle>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TransportersVehicle> TransportersVehicles { get; set; }
    }
}
