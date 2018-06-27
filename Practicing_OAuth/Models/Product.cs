//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Practicing_OAuth.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string SlugURL { get; set; }
        [DisplayName("Specifications")]
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Description")]
        public string Product_Description { get; set; }
        public string Specifications { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
