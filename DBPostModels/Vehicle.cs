using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApiServices.DBPostModels;

public partial class Vehicle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Type { get; set; }

    public string Number { get; set; } = null!;

    public int Owner { get; set; }

    public string? TrailerNumber { get; set; }

    public virtual Requisite OwnerNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual VehicleType TypeNavigation { get; set; } = null!;
}
