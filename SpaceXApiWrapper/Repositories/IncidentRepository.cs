using SpaceXApiWrapper.Models;
using System.Reflection;

namespace SpaceXApiWrapper.Repositories
{
    public interface IIncidentRepository
    {
        public List<Incident> GetIncidents();
        public Incident GetIncident(Guid id);
        public Incident AddIncident(Incident incident);
        public Incident UpsertIncident(Incident incident);
        void DeleteIncident(Guid id);
    }

    public class IncidentRepository : IIncidentRepository
    {
        public List<Incident> GetIncidents()
        {
            using (var context = new ApiContext())
            {
                var list = context.Incidents.ToList();
                return list;
            }
        }
        public Incident GetIncident(Guid id)
        {
            using (var context = new ApiContext())
            {
                Incident? incident = context.Incidents.Find(id);
                return incident;
            }
        }
        public Incident AddIncident(Incident incident)
        {
            using (var context = new ApiContext())
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
                return incident;
            }
        }
        public Incident UpsertIncident(Incident incident)
        {
            using (var context = new ApiContext())
            {
                if (incident == null)
                {
                    return null;
                }
                Incident? result = context.Incidents.Find(incident.Id);
                if (result == null)
                {
                    context.Incidents.Add(incident);
                    context.SaveChanges();
                    return incident;
                }

                Type resType = incident.GetType();
                Type incType = result.GetType();
                if (resType != incType)
                {
                    return result;
                }
                foreach (PropertyInfo incProp in incType.GetProperties())
                {
                    if (!incProp.CanRead) continue;
                    if (incProp.Name == "id") continue;
                    PropertyInfo? targetProp = resType.GetProperty(incProp.Name);
                    if (targetProp == null) continue;
                    if (!incProp.CanWrite) continue;
                    if (targetProp.GetSetMethod(true) != null && targetProp.GetSetMethod(true).IsPrivate)
                    {
                        continue;
                    }
                    if ((targetProp.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                    {
                        continue;
                    }
                    if (!targetProp.PropertyType.IsAssignableFrom(incProp.PropertyType))
                    {
                        continue;
                    }
                    targetProp.SetValue(result, incProp.GetValue(incident, null), null);
                    context.SaveChanges();
                }
                return result;
            }
        }
        public void DeleteIncident(Guid id)
        {
            using (var context = new ApiContext())
            {
                Incident? result = context.Incidents.Find(id);
                if (result != null)
                {
                    context.Incidents.Remove(result);
                    context.SaveChanges();
                }
            }
        }
    }
}
