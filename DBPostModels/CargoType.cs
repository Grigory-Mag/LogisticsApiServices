using System;
using System.Collections.Generic;

namespace LogisticsApiServices.DBPostModels
{
    public partial class CargoType
    {
        public CargoType()
        {
            Cargos = new HashSet<Cargo>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Cargo> Cargos { get; set; }
    }
}
