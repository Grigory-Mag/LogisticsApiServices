using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApiServices.DBPostModels;

public partial class Requisite
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string LegalAddress { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string Ceo { get; set; } = null!;

    public int Pts { get; set; }

    public int Role { get; set; }

    public string Name { get; set; } = null!;

    public int? Type { get; set; }

    public virtual ICollection<Request> RequestCustomerNavigations { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestTransporterNavigations { get; set; } = new List<Request>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual RequisitesType? TypeNavigation { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
