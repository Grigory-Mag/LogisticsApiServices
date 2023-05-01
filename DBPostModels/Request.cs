using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Request
{
    public int Id { get; set; }

    public int? Vehicle { get; set; }

    public double? Price { get; set; }

    public DateTime CreationDate { get; set; }

    public bool? DocumentsOriginal { get; set; }

    public int? Customer { get; set; }

    public int? Transporter { get; set; }

    public int? Cargo { get; set; }

    public bool? IsFinishied { get; set; }

    public int? Driver { get; set; }

    public virtual Cargo? CargoNavigation { get; set; }

    public virtual Requisite? CustomerNavigation { get; set; }

    public virtual Driver? DriverNavigation { get; set; }

    public virtual Requisite? TransporterNavigation { get; set; }

    public virtual Vehicle? VehicleNavigation { get; set; }

    public virtual ICollection<Route> IdRoutes { get; set; } = new List<Route>();
}
