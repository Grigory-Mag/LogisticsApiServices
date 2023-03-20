using System;
using System.Collections.Generic;

namespace GrpcGreeter.DBPostModels
{
    public partial class Order
    {
        public Order()
        {
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Cargo { get; set; }
        public int ColumnName { get; set; }

        public virtual Cargo? CargoNavigation { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
