namespace TravelPlanning.Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trip")]
    public partial class Trip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trip()
        {
            TripDetail = new HashSet<TripDetail>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        public Guid User_id { get; set; }

        public DateTime? Started_time { get; set; }

        public DateTime? Ended_time { get; set; }

        public int? Days { get; set; }

        [StringLength(500)]
        public string Cover { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TripDetail> TripDetail { get; set; }
    }
}
