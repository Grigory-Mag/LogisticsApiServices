using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Request
    {
        public int Id { get; set; }
        public int? Vehicle { get; set; }
        public double? Price { get; set; }
        public int? Order { get; set; }
        public bool Conditions { get; set; }

        public virtual Order? OrderNavigation { get; set; }
        public virtual Vehicle? VehicleNavigation { get; set; }
    }
}
