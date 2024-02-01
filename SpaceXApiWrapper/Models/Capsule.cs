namespace SpaceXApiWrapper.Models
{
    public class Capsule
    {
        // dragon, landings, missions, and original_launch all return "null" from the SpaceX API
        public string id { get; set; }
        public string type { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public int reuse_count { get; set; } = 0;
    }
}
