//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PatternMaker
{
    using System;
    using System.Collections.Generic;
    
    public partial class ControllerUser
    {
        public ControllerUser()
        {
            this.ControllerNames = new HashSet<ControllerName>();
        }
    
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<ControllerName> ControllerNames { get; set; }
    }
}
