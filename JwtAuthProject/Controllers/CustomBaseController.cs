﻿
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace JwtAuthProject.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult ActionResultInstance<T>(Response<T> response)where T : class
        {


            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

    }
}
