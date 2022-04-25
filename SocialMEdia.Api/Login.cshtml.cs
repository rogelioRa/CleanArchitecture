using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using TEC.SB.Core.Models.Modifications;

namespace TEC.SB.UI.Colaboradores.Dev.Pages
{
    public class LoginModel : PageModel
    {

        public User user { get; set; }
        public LoginModel(User user)
        {
            this.user = user;
        }
     
    }
}
