using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Driver
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public bool Sanitation { get; set; }

    public int Licence { get; set; }

    public virtual DriverLicence LicenceNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
