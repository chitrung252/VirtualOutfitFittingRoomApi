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
    
    public partial class AccountViewModel : DataService.ViewModels.BaseEntityViewModel<DataService.Models.Entities.Account>
    {
    	
    			public virtual int AccountID { get; set; }
    			public virtual string FirstName { get; set; }
    			public virtual string LastName { get; set; }
    			public virtual string Address { get; set; }
    			public virtual string PhoneNumber { get; set; }
    			public virtual string Username { get; set; }
    			public virtual string Password { get; set; }
    			public virtual Nullable<System.DateTime> DateCreated { get; set; }
    			public virtual Nullable<System.DateTime> DateModified { get; set; }
    			public virtual Nullable<bool> IsActived { get; set; }
    			public virtual Nullable<bool> IsDeleted { get; set; }
    			public virtual Nullable<int> RoleID { get; set; }
    	
    	public AccountViewModel() : base() { }
    	public AccountViewModel(DataService.Models.Entities.Account entity) : base(entity) { }
    
    }
}
