using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Requisite
{
    public int Id { get; set; }

    public int? Ownership { get; set; }

    public string? LegalAddress { get; set; }

    public string? Inn { get; set; }

    public string? Ceo { get; set; }

    public int? Pts { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Ownership? OwnershipNavigation { get; set; }
}
