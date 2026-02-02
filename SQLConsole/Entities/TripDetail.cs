namespace SQLConsole.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TripDetail")]
    public partial class TripDetail
    {
        public Guid Id { get; set; }

        public Guid Trip_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public DateTime? Startime { get; set; }

        [StringLength(50)]
        public string Place_id { get; set; }

        public virtual Trip Trip { get; set; }
    }
}
