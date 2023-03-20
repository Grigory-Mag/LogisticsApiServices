using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class CargoConstraint
    {
        public int IdConstraint { get; set; }
        public int IdCargo { get; set; }

        public virtual Cargo IdCargoNavigation { get; set; } = null!;
        public virtual Constraint IdConstraintNavigation { get; set; } = null!;
    }
}
