using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceXApiWrapper.Models;
using SpaceXApiWrapper.Repositories;

namespace SpaceXApiWrapper.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        readonly IIncidentRepository _incidentRepository;
        public IncidentController(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        [Authorize(Roles = "Admin,Reader")]
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

        [Authorize(Roles = "Admin,Reader")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
