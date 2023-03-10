//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int order_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public string user_phone { get; set; }
        public string user_address { get; set; }
        public string user_note { get; set; }
        public Nullable<double> total { get; set; }
        public string order_code { get; set; }
        public Nullable<int> order_status { get; set; }
        public Nullable<System.DateTime> order_createdate { get; set; }
        public Nullable<System.DateTime> order_updatedate { get; set; }
        public string buyer_name { get; set; }
        public string phone { get; set; }
        public string addressto { get; set; }
        public string note { get; set; }
        public Nullable<double> customer_pay { get; set; }
        public Nullable<int> type { get; set; }
        public string keycode { get; set; }
        public Nullable<int> discount { get; set; }
        public string refuse { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
