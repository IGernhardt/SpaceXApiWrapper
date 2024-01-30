using Microsoft.AspNetCore.Mvc;
using SpaceXApiWrapper.Models;
using SpaceXApiWrapper.Repositories;

namespace SpaceXApiWrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        readonly IIncidentRepository _incidentRepository;
        public IncidentController(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        [HttpGet]
        public ActionResult<List<Incident>> Get()
        {
            try
            {
                return Ok(_incidentRepository.GetIncidents());
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Incident> GetIncident(Guid id)
        {
            try
            {
                var result = _incidentRepository.GetIncident(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
