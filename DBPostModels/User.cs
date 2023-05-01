using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Patroynymic { get; set; }
}
