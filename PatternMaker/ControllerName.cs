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
    
    public partial class ControllerName
    {
        public ControllerName()
        {
            this.StateTransitions = new HashSet<StateTransition>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IncludeMessageText { get; set; }
        public bool IncludeAvailabilityProfile { get; set; }
        public string Namespace { get; set; }
        public string UserEmail { get; set; }
        public bool UseInnerClasses { get; set; }
    
        public virtual ControllerUser ControllerUser { get; set; }
        public virtual ICollection<StateTransition> StateTransitions { get; set; }
    }
}
