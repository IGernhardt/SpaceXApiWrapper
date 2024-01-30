

using Microsoft.EntityFrameworkCore;
using SpaceXApiWrapper.Models;

namespace SpaceXApiWrapper
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "IncidentReports");
        }
        public DbSet<Incident> IncidentReports { get; set; }
    }
}
