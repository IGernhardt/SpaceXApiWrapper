using Microsoft.AspNetCore.Mvc;
using SpaceXApiWrapper.Models;
using SpaceXApiWrapper.Repositories;

namespace SpaceXApiWrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        readonly IIncidentRepository _incidentRepository;
        public AdminController(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        [HttpGet]
        public ActionResult<List<Incident>> Get()
        {
            try
            {
                List<Incident> result = _incidentRepository.GetIncidents();
                return Ok(result);
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
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<Incident> AddIncident(Incident incident)
        {
            try
            {
                if (incident == null)
                {
                    return BadRequest();
                }
                Incident result = _incidentRepository.GetIncident(incident.Id);
                if (result != null)
                {
                    return BadRequest("Incident Already Filed");
                }
                Incident createdIncident = _incidentRepository.AddIncident(incident);
                return Ok(createdIncident);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error Creating New Incident");
            }
        }

        [HttpPut]
        public ActionResult<Incident> UpsertIncident(Incident incident)
        {
            try
            {
                if (incident == null)
                {
                    return BadRequest();
                }
                Incident result = _incidentRepository.UpsertIncident(incident);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error Uppdating Incident");
            }
        }

        [HttpDelete("{id:guid}")]
        public ActionResult DeleteIncident(Guid id)
        {
            try
            {
                var result = _incidentRepository.GetIncident(id);
                if (result == null)
                {
                    return NotFound();
                }
                _incidentRepository.DeleteIncident(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error Deleting Incident");
            }
        }
    }
}
