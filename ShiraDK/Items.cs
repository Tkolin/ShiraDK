//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShiraDK
{
    using System;
    using System.Collections.Generic;
    
    public partial class Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Items()
        {
            this.ItemsForEvents = new HashSet<ItemsForEvents>();
            this.WareHouse = new HashSet<WareHouse>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<double> Height { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> Count { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemsForEvents> ItemsForEvents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WareHouse> WareHouse { get; set; }
    }
}
