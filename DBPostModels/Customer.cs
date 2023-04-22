using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Customer
{
    public int Id { get; set; }

    public int Requisite { get; set; }

    public int Cargo { get; set; }

    public virtual Cargo CargoNavigation { get; set; } = null!;

    public virtual Requisite RequisiteNavigation { get; set; } = null!;
}
