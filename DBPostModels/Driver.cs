using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Driver
    {
        public Driver()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Surname { get; set; }
        public string? Patronymic { get; set; }
        public bool Sanitation { get; set; }
        public int? Licence { get; set; }
        public int ColumnName { get; set; }

        public virtual DriverLicence? LicenceNavigation { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
