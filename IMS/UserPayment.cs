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
    
    public partial class UserPayment
    {
        public long CustomerId { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public System.TimeSpan PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public long Amount { get; set; }
    }
}