//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        public string ProductName { get; set; }
        public bool isDiscontinued { get; set; }
        public long Quantity { get; set; }
        public string Product_Details { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
