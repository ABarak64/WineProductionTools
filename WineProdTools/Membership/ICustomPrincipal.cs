using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WineProdTools.Membership
{
    public interface ICustomPrincipal : System.Security.Principal.IPrincipal
    {
        string UserId { get; set; }
        Int64 AccountId { get; set; }
    }
}