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
    
    public partial class Import
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Import()
        {
            this.ImportDetails = new HashSet<ImportDetail>();
        }
    
        public int id { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> importunit_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportDetail> ImportDetails { get; set; }
        public virtual ImportUnit ImportUnit { get; set; }
    }
}
