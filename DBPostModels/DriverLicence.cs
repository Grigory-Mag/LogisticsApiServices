using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApiServices.DBPostModels;

public partial class DriverLicence
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? Series { get; set; }

    public int? Number { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
