using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class DriverLicence
    {
        public DriverLicence()
        {
            Drivers = new HashSet<Driver>();
        }

        public int Id { get; set; }
        public int? Series { get; set; }
        public int? Number { get; set; }
        public DateOnly? Date { get; set; }
        public int ColumnName { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
    }
}
