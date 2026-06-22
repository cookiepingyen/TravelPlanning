namespace TravelPlanning.Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TripDays
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TripDays()
        {
            TripDayPlace = new HashSet<TripDayPlace>();
        }

        public Guid Id { get; set; }

        public Guid Trip_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public DateTime? Startime { get; set; }

        [StringLength(50)]
        public string Place_id { get; set; }

        public virtual Trip Trip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TripDayPlace> TripDayPlace { get; set; }
    }
}
