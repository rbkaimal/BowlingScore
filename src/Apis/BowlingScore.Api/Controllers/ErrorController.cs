using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
