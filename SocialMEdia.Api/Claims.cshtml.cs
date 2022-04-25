using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TEC.SB.UI.Colaboradores.Dev.Pages
{
    public class ClaimsModel : PageModel
    {
        private readonly ILogger<ClaimsModel> _logger;
        public ClaimsModel(ILogger<ClaimsModel> logger)
        {
            this._logger = logger;
        }
        
    }
}
