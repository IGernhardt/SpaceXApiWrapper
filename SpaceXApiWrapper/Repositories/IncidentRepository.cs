using Microsoft.EntityFrameworkCore;
using SpaceXApiWrapper.Models;

namespace SpaceXApiWrapper.Repositories
{
    public interface IIncidentRepository
    {
        public List<Incident> GetIncidents();
        public Incident GetIncident(Guid id);
    }

    public class IncidentRepository : IIncidentRepository
    {
        public IncidentRepository()
        {
            using (var context = new ApiContext())
            {
                var incidents = new List<Incident>
                {
                    new Incident
                    {
                       Id = Guid.NewGuid(),
                       SubmitDate = new DateOnly(),
                       Name = "Report 1",
                       Description = "This is the first report.",
                    },
                    new Incident
                    {
                       Id = Guid.NewGuid(),
                       SubmitDate = new DateOnly(),
                       Name = "Report 2",
                       Description = "This is the second report.",
                    }
                };

                context.IncidentReports.AddRange(incidents);
                context.SaveChanges();
            }
        }
        public List<Incident> GetIncidents()
        {
            using (var context = new ApiContext())
            {
                var list = context.IncidentReports.ToList();
                return list;
            }
        }
        public Incident GetIncident(Guid id)
        {
            using (var context = new ApiContext())
            {
                Incident? incident = context.IncidentReports.Find(id);
                return incident;
            }
        }
    }
}
