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
    
    public partial class Events
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Events()
        {
            this.BuyingTickets = new HashSet<BuyingTickets>();
            this.ItemsForEvents = new HashSet<ItemsForEvents>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime DateStart { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public int OrganizerID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> NumberOfSeats { get; set; }
        public Nullable<int> AvailableOfSeats { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyingTickets> BuyingTickets { get; set; }
        public virtual Organizers Organizers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemsForEvents> ItemsForEvents { get; set; }
    }
}
