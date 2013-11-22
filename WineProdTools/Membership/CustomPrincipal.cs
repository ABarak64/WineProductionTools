using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace WineProdTools.Membership
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
 
        public CustomPrincipal(string username)
	    {
		    this.Identity = new GenericIdentity(username);
	    }
 
	    public bool IsInRole(string role)
	    {
		    return Identity != null && Identity.IsAuthenticated && 
		       !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
	    }

	    public string UserId { get; set; }
	    public Int64 AccountId { get; set; }
   
    }
}