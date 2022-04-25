using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using TEC.SB.Core.Interfaces.Services.Modifications;
using TEC.SB.Core.Models;
using TEC.SB.Core.Models.Modifications;
using TEC.SB.Data.DataContext;
using TEC.SB.Services.Services.Modifications;
using TEC.SB.UI.Dev.Middlewares;

namespace TEC.SB.UI.Dev.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        const string relayStateReturnUrl = "ReturnUrl";
        private readonly Saml2Configuration config;

        private readonly IUserService userService;

        private readonly Messages messages;

        public AuthController(IOptions<Saml2Configuration> configAccessor, IUserService userService)
        {
            config = configAccessor.Value;
            this.userService = userService;
            messages = new Messages();
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if(Env.isDevelopment)
            {
                // here login with our auth
                return Redirect(Url.Content("~/auth/login-development"));
            }
            else
            {
                // here login with federation
                var binding = new Saml2RedirectBinding();
                binding.SetRelayStateQuery(new Dictionary<string, string> { { relayStateReturnUrl, returnUrl ?? Url.Content("~/") } });

                return binding.Bind(new Saml2AuthnRequest(config)).ToActionResult();
            }
        }

        [Route("AssertionConsumerService")]
        public async Task<IActionResult> AssertionConsumerService()
        {
            try
            {
                var binding = new Saml2PostBinding();
                var saml2AuthnResponse = new Saml2AuthnResponse(config);
                   binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                {
                    throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
                } 
                ClaimsPolicy claimsPolicy = new ClaimsPolicy(userService);
                binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal, claimsPolicy));

                var relayStateQuery = binding.GetRelayStateQuery();
                var returnUrl = relayStateQuery.ContainsKey(relayStateReturnUrl) ? relayStateQuery[relayStateReturnUrl] : Url.Content("~/claims");

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if(Env.isDevelopment)
                    {
                        await HttpContext.SignOutAsync();
                    }
                    else
                    {
                        
                        var binding = new Saml2RedirectBinding();
                        var saml2LogoutRequest = await new Saml2LogoutRequest(config, User).DeleteSession(HttpContext);
                        return binding.Bind(saml2LogoutRequest).ToActionResult();
                    }
                }
                Env.user = null;
                Env.userProfile = null;
                return Redirect(Url.Content("~/"));
            }
            catch (System.Exception ex)
            {
                
               return Json(new Response<Object>()
                {
                    message = messages.US_LOGOUT,
                    status = messages.SUCCESS_STATUS,
                    data = null,
                    error = ex
                });
            }
        }

        [HttpGet]
        [Route("GetClaims")]
        public Dictionary<string, string> GetClaims()
        {
            Dictionary<string, string> response = new Dictionary<string, string>();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    foreach (var claim in User.Claims)
                    {
                        string key = claim.Type.Split('/')[claim.Type.Split('/').Length - 1];
                        if (!response.ContainsKey(key))
                            response.Add(key, claim.Value);
                        else
                            response[key] = response[key] + "#" + claim.Value;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        [Route("/logout")]
        public IActionResult LogoutView()
        {
            return View("~/Pages/Logout.cshtml");
        }

        [Route("/Claims")]
        [Authorize]
        public IActionResult Claims()
        {
            return View("~/Pages/Claims.cshtml");
        }

        [Route("/")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/inicio"));
            }
            else 
            {
               return Redirect(Url.Content("~/auth/login"));
            }
        }

        [HttpGet]
        [Route("Profile")]
        [CustomAuthorizationAttribute]
        public async Task<IActionResult> GetProfileUser()
        {
            var response = await userService.Profile(User.Identity.Name);
            return Json(new {response = response, UserFederation = User.Identity});
        }

        [HttpGet]
        [Route("login-development")]
        public IActionResult LoginViewDevelopment()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/inicio"));
            }
            else
            {
                return View("~/Pages/Login.cshtml");
            }
        }

        [HttpPost]
        [Route("auth-development")]
        public async Task<IActionResult> LoginDevelopment([FromForm] User model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Pages/Login.cshtml", model);       
            }
            Response<User> response = await userService.FindUserAuth(model.Name);
            if (response.status == 200)
            {
                User user = response.data.ElementAt(0);
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
                    new Claim(ClaimTypes.Email, $"{user.Name}@itesm.mx"),
                    new Claim(ClaimTypes.Role, user.Profile.Extension),
                    new Claim(ClaimTypes.Actor, user.Name),
                    new Claim("NameRole", user.Profile.Name),
                    new Claim(ClaimTypes.WindowsAccountName, $"TEC/{user.Name}"),
                };

                ClaimsPolicy claimsPolicy = new ClaimsPolicy(userService);
                var claimsPolicies = claimsPolicy.GetClaimsPolicies(user);
                if (claimsPolicies.Count() > 0)
                {
                    claims.AddRange(claimsPolicies);
                }

                var gramaIdentity = new ClaimsIdentity(claims, "Development Identity", ClaimTypes.NameIdentifier, ClaimTypes.Role);
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Development Identity", ClaimTypes.NameIdentifier, ClaimTypes.Role)
                {
                    BootstrapContext = ((ClaimsIdentity)gramaIdentity).BootstrapContext
                });

                await HttpContext.SignInAsync(userPrincipal);

            }
            else if(response.status == 404)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Name),
                    new Claim(ClaimTypes.NameIdentifier, model.Name),
                    new Claim(ClaimTypes.Email, $"{model.Name}@itesm.mx"),
                    new Claim(ClaimTypes.Role, "Notfound"),
                    new Claim(ClaimTypes.Actor, model.Name),
                    new Claim("NameRole", "NotFound"),
                    new Claim(ClaimTypes.WindowsAccountName, $"TEC/{model.Name}"),
                };

                ClaimsPolicy claimsPolicy = new ClaimsPolicy(userService);
                var claimsPolicies = claimsPolicy.GetClaimsPolicies(null);
                if (claimsPolicies.Count() > 0)
                {
                    claims.AddRange(claimsPolicies);
                }

                var gramaIdentity = new ClaimsIdentity(claims, "Development Identity", ClaimTypes.NameIdentifier, ClaimTypes.Role);
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Development Identity", ClaimTypes.NameIdentifier, ClaimTypes.Role)
                {
                    BootstrapContext = ((ClaimsIdentity)gramaIdentity).BootstrapContext
                });
                await HttpContext.SignInAsync(userPrincipal);
            }
            else
            {
                return Json(response);  
            }
            return Redirect(Url.Content("~/"));
        }
    }
}

