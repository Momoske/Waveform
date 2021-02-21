using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waveform.Controllers
{
  public class ErrorController : Controller
  {
    [Route("Error/{code}")]
    public IActionResult HttpStatusCodeHandler(int code)
    {
      switch(code)
      {
        case 404:
          ViewBag.ErrorMessage = "We couldn't find where you want to go.";
          break;
      }
      return View("NotFound");
    }
  }
}
