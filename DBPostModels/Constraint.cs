using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Constraint
    {
        public Constraint()
        {
            CargoConstraints = new HashSet<CargoConstraint>();
        }

        public int Id { get; set; }
        public string? Desc { get; set; }
        public int ColumnName { get; set; }

        public virtual ICollection<CargoConstraint> CargoConstraints { get; set; }
    }
}
