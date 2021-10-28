using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginExampleServer.Data;
using LoginExampleServer.Data.Impl;
using LoginExampleServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginExampleServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdultsController : ControllerBase
    {
        private IAdultService _adultData;

        public AdultsController(IAdultService peopleData)
        {
            this._adultData = peopleData;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Adult>>> GetAdults([FromQuery] string? gender, [FromQuery] int? adultId)
        {
            try
            {
                IList<Adult> adults = await _adultData.GetAdultsAsync();
                IList<Adult> filteredAdults = new List<Adult>();
                if (gender == null && adultId==null)
                {
                    filteredAdults = adults;
                }
                else if (adultId != null)
                {
                    foreach(Adult adult in adults)
                    {
                        if(adult.Id==adultId)filteredAdults.Add(adult);
                    }
                }
                else
                {
                    foreach(Adult adult in adults)
                    {
                        if(adult.Sex.Equals(gender))filteredAdults.Add(adult);
                    }
                }
                return Ok(filteredAdults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Adult>> AddAdult([FromBody] Adult adult)
        {
            try
            {
                await _adultData.AddPersonAsync(adult);
                return Created($"/{adult.Id}", adult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteAdult([FromRoute] int id)
        {
            try
            {
                await _adultData.RemovePersonAsync(id);
                return Ok(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}