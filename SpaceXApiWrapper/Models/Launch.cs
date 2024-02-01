namespace SpaceXApiWrapper.Models
{
    public class Launch
    {
        // Params that return "null": 
        // - IsTentative
        // - LaunchSite
        // - StaticFireDateUTC
        // - Telemetry
        // - TentativeMaxPrecision
        // Params that return "invalid JSON response / internal server error"
        // - Ships
        public string id { get; set; } = string.Empty;
        public string details { get; set; } = string.Empty;
        public DateTime launch_date_utc { get; set; }
        public List<string> mission_id { get; set; } = [];
        public string mission_name { get; set;} = string.Empty;
        public LaunchRocket? rocket { get; set; }
        public bool upcoming { get; set; } = false;
    }
}
