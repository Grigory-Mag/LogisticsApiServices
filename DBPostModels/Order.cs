using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int? Cargo { get; set; }

    public virtual Cargo? CargoNavigation { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
