namespace SpaceXApiWrapper.Models
{
    public class SecondStage
    {
        public int burn_time_sec { get; set; } = 0;
        public int engines { get; set; } = 0;
        public double fuel_amount_tons { get; set; } = 0;
        public Force? thrust { get; set; }
    }
}