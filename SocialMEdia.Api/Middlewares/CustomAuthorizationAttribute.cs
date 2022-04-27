using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace TEC.SB.UI.Dev.Middlewares
{
  public class CustomAuthorizationAttribute: ActionFilterAttribute, IActionFilter
  {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
          // Do something before the action executes.
          if(!context.HttpContext.User.Identity.IsAuthenticated)
          {
            context.Result = new BadRequestObjectResult(new {status = 401, messages =  "Unauthorized"});
            return;
          }
      }

      
  }
}
