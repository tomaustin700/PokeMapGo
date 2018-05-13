using PokeMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PokeMap.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {

        // Post api/<controller>
        public void Post([FromBody]Sighting value)
        {
            if (value != null)
            {
                var sightingsController = new SightingsController();
                sightingsController.APIAdd(value);
            }
        }

      
    }
}