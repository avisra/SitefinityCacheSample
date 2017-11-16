using Avisra.Samples.Hogwarts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Avisra.Samples.Hogwarts.Api
{
    [RoutePrefix("h-api")]
    public class HouseController : ApiController
    {
        HouseRepository repository;

        public HouseController(HouseRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("houses")]
        public IHttpActionResult Get()
        {
            // Normally I would setup a data transfer object (DTO) instead of returning back the domain model, but no need here
            return this.Ok(this.repository.GetHouses());
        }
    }
}
