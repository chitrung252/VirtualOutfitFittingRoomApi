//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataService.ViewModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductImageViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.ProductImage>
    {
    	
    			public virtual int ImageID { get; set; }
    			public virtual string ImageUrl { get; set; }
    			public virtual Nullable<System.DateTime> DateCreated { get; set; }
    			public virtual Nullable<System.DateTime> DateModified { get; set; }
    			public virtual Nullable<bool> IsActived { get; set; }
    			public virtual Nullable<bool> IsDeleted { get; set; }
    			public virtual Nullable<int> ProductID { get; set; }
    	
    	public ProductImageViewModel() : base() { }
    	public ProductImageViewModel(DataService.Models.Entities.ProductImage entity) : base(entity) { }
    
    }
}
