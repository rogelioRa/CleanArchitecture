using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TEC.SB.UI.Dev.Middlewares
{
    public class AuthorizationClaimAttribute: ActionFilterAttribute, IActionFilter
    {
        public string action { get; set; }
        public AuthorizationClaimAttribute()
        {
            this.action = "";
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
          // Do something before the action executes.
          if(!context.HttpContext.User.Identity.IsAuthenticated)
          {
                context.Result = new BadRequestObjectResult((status: 401, messages: "Unauthorized"));
                return;
          }
          var claims = context.HttpContext.User.Claims;
          if(!this.HasClaim(this.action, claims))
          {
                context.Result = new BadRequestObjectResult((status: 401, messages: $"Unauthorized, you don´t have permission to access this service! Missing {this.action} policy"));
                return;
          }
          
        }

        private bool HasClaim(string action, IEnumerable<Claim> claims)
        {
            bool hasClaim = false;
            foreach(var claim in claims)
            {
                if(claim.Value == action)
                {
                    hasClaim = true;
                }
            }
            return hasClaim;
        }

        
    }
}