//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Power_Billing_Portal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill
    {
        public int BillID { get; set; }
        public int UserID { get; set; }
        public string CompanyName { get; set; }
        public int Price { get; set; }
        public System.DateTime Date { get; set; }
        public string LastDate { get; set; }
        public Nullable<int> indexx { get; set; }
    
        public virtual User User { get; set; }
    }
}
