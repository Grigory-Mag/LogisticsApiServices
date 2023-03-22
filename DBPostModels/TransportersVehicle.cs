using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class TransportersVehicle
    {
        public int IdTransporter { get; set; }
        public int IdVehicle { get; set; }
        public int? Test { get; set; }

        public virtual Transporter IdTransporterNavigation { get; set; } = null!;
        public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
    }
}
