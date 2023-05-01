using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApiServices.DBPostModels;

public partial class Cargo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? Type { get; set; }

    public double? Weight { get; set; }

    public double? Volume { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public string? Constraints { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual CargoType? TypeNavigation { get; set; }
}
