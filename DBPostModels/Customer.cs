using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int Requisite { get; set; }
        public int Cargo { get; set; }
        public int ColumnName { get; set; }

        public virtual DBConverter CargoNavigation { get; set; } = null!;
        public virtual Requisite RequisiteNavigation { get; set; } = null!;
    }
}
