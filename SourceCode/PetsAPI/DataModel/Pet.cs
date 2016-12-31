//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pet
    {
        public Pet()
        {
            this.Articles = new HashSet<Article>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int ImageId { get; set; }
        public int UserAuthInfoId { get; set; }
    
        public virtual ICollection<Article> Articles { get; set; }
        public virtual Image Image { get; set; }
        public virtual UserAuthInfo UserAuthInfo { get; set; }
    }
}