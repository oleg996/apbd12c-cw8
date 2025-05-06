using System.Runtime.CompilerServices;
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
         public async Task<IActionResult> Add_client(String FirstName,String LastName, String Email,String phone,String pesel){
            if (!new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z").IsMatch(Email))
                return NotFound("incorect email");
            if (!new Regex(@"^\d{11}$").IsMatch(pesel))
                return NotFound("incorect pessel");

            if(await _tripsService.GetClientID(FirstName,LastName,Email,phone,pesel) != -1)
                return NotFound("such client exists");



            await _tripsService.AddClient(FirstName,LastName,Email,phone,pesel);


            int id  = await _tripsService.GetClientID(FirstName,LastName,Email,phone,pesel);

            return Ok(id);
         }

        [HttpPut("{id}/trips/{tripid}")]

        public async Task<IActionResult> AddTrip(int id,int tripid){

            if ( await _tripsService.DoesClientExists(id))
                return NotFound("No such client");

            if ( await _tripsService.DoesTripExistByTripId(tripid))
                return NotFound("No such trip");

            if (await _tripsService.DoesTripFullByTripId(tripid))
                return NotFound("Trip is full");


            await _tripsService.AddTrip(id,tripid);

            return Ok();
        }
        [HttpDelete("{id}/trips/{tripid}")]
        public async Task<IActionResult> RemoveTrip(int id,int tripid){


            if ( await _tripsService.DoesTripExistByTripIdAndClientId(tripid,id))
                return NotFound("No such trip");




            await _tripsService.DeletetByTripIdAndClientId(tripid,id);
            return Ok();
        }

    

    }
}
