namespace TravelPlanning.Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FavoriteItem")]
    public partial class FavoriteItem
    {
        public Guid Id { get; set; }

        public Guid Favorite_id { get; set; }

        [StringLength(50)]
        public string Place_id { get; set; }

        public virtual Favorite Favorite { get; set; }
    }
}
