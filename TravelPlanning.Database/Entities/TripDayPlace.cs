namespace TravelPlanning.Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TripDayPlace")]
    public partial class TripDayPlace
    {
        public Guid Id { get; set; }

        public Guid TripDays_id { get; set; }

        public DateTime Travel_time { get; set; }

        [Required]
        [StringLength(50)]
        public string Place_id { get; set; }

        public int Transit_time { get; set; }

        [Required]
        [StringLength(30)]
        public string Place_name { get; set; }

        public int Stay_time { get; set; }

        public virtual TripDays TripDays { get; set; }
    }
}
