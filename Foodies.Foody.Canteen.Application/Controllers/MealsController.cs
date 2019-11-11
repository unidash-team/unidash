using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Foodies.Foody.Canteen.Core.Data;
using Foodies.Foody.Canteen.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foodies.Foody.Canteen.Application.Controllers
{
    [ApiController]
    [Route("meals")]
    public class MealsController : ControllerBase
    {
        private readonly ICanteenService _canteenService;

        public MealsController(ICanteenService canteenService)
        {
            _canteenService = canteenService;
        }

        [HttpGet]
        [ResponseCache(Duration = 1)]
        [ProducesResponseType(typeof(IList<Meal>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get() 
            => Ok(await _canteenService.GetMealsAsync());

        // TODO: Normalize output from dictionary to list
        [HttpGet("week")]
        //[ResponseCache(Duration = 1)]
        [ProducesResponseType(typeof(IEnumerable<MealsDateTuple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWeek() 
            => Ok(await _canteenService.GetMealsOfWeekAsync());
    }
}
