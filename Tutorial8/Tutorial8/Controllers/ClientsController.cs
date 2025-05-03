using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public ClientsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTrip(int id)
        {
            if( await _tripsService.DoesClientExists(id)){
              return NotFound("No such client");
             }


             if( await _tripsService.DoesTripExist(id)){
              return NotFound("No trips for such client");
             }
             var trip = await _tripsService.GetTrip(id);
             Console.WriteLine(trip);
            return Ok(trip);
        }


        [HttpPost]
         public async Task<IActionResult> Add_client(String FirstName,String LastName, String Email,String pesel){
            if (!new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z").IsMatch(Email))
                return NotFound();
            if (!new Regex(@"^\d{11}$").IsMatch(pesel))
                return NotFound();


            return Ok();
         }
    }
}
