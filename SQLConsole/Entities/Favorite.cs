namespace SQLConsole.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Favorite")]
    public partial class Favorite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Favorite()
        {
            FavoriteItem = new HashSet<FavoriteItem>();
        }

        public Guid Id { get; set; }

        public Guid User_id { get; set; }

        [StringLength(10)]
        public string Name { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavoriteItem> FavoriteItem { get; set; }
    }
}
