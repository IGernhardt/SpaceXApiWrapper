namespace SpaceXApiWrapper.Models
{
    public class Incident
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? LaunchId { get; set; }
        public bool? IsActive { get; set; }
        // !!! ADD: more fields as necessary
    }
}
