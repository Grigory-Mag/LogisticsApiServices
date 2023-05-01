using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApiServices.DBPostModels;

public partial class Route
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public int Action { get; set; }

    public DateTime ActionDate { get; set; }

    public virtual RouteAction ActionNavigation { get; set; } = null!;

    public virtual ICollection<Request> IdRequests { get; set; } = new List<Request>();
}
