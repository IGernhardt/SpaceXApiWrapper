namespace SpaceXApiWrapper.Models
{
    public class Rocket
    {
        public string id { get; set; } = string.Empty;
        public bool active { get; set; } = false;
        public string company { get; set; } = string.Empty;
        public int cost_per_launch { get; set; } = 0;
        public string country { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int boosters { get; set; } = 0;
        public FirstStage? first_stage { get; set; }
        public LandingLegs? landing_legs { get; set; }
        public Mass? mass { get; set; }
        public string name { get; set; } = string.Empty;
        public FirstStage? second_stage { get; set; }
        public List<PayloadWeight>? payload_weights { get; set; }
        public int stages { get; set; } = 0;
        public int success_rate_pct { get; set; } = 0;
        public string type { get; set; } = string.Empty;
    }
}
