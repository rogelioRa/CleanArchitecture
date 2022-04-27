using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TEC.SB.UI.Dev.Middlewares
{
    public class AuthorizationRoleAttribute: ActionFilterAttribute, IActionFilter
    {
        public string role { get; set; }
        public AuthorizationRoleAttribute()
        {
            this.role = "";
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
          // Do something before the action executes.
          if(!context.HttpContext.User.Identity.IsAuthenticated)
          {
            context.Result = new BadRequestObjectResult(new {status = 401, messages =  "Unauthorized"});
            return;
          }
          var claims = context.HttpContext.User.Claims;
          if(!this.HasRole(this.role, claims))
          {
            context.Result = new BadRequestObjectResult(new {status = 401, messages =  $"Unauthorized, you don´t have permission to access this service! Missing {this.role} Role!"});
            return;
          }
          
        }

        private bool HasRole(string role, IEnumerable<Claim> claims)
        {
            bool hasRole = false;
            foreach(var claim in claims)
            {
                if(claim.Type == ClaimTypes.Role && claim.Value == role)
                {
                    hasRole = true;
                }
            }
            return hasRole;
        }

        
    }
}