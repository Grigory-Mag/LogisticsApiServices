﻿using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Requisite
{
    public int Id { get; set; }

    public string LegalAddress { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string Ceo { get; set; } = null!;

    public int Pts { get; set; }

    public int Role { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Request> RequestCustomerNavigations { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestTransporterNavigations { get; set; } = new List<Request>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
