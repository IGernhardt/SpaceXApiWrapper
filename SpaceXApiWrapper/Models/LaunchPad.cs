namespace SpaceXApiWrapper.Models
{
    public class LaunchPad
    {
        // Params that return "null": 
        // - attempted_launches
        // - location
        // - successful_launches
        // Params that return "Internal Server Error"
        // - rocket
        public string details { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string wikipedia { get; set; } = string.Empty;
    }
}
