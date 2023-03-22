using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Transporter
    {
        public Transporter()
        {
            VehiclesTransporters = new HashSet<VehiclesTransporter>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<VehiclesTransporter> VehiclesTransporters { get; set; }
    }
}
