﻿using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Requests = new HashSet<Request>();
            Transporters = new HashSet<Transporter>();
            TransportersNavigation = new HashSet<Transporter>();
        }

        public int Id { get; set; }
        public int? Type { get; set; }
        public string? Number { get; set; }
        public int? Owner { get; set; }
        public int? Driver { get; set; }

        public virtual Driver? DriverNavigation { get; set; }
        public virtual Ownership? OwnerNavigation { get; set; }
        public virtual VehicleType? TypeNavigation { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<Transporter> Transporters { get; set; }
        public virtual ICollection<Transporter> TransportersNavigation { get; set; }
    }
}
