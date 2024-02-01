﻿namespace SpaceXApiWrapper.Models
{
    public class FirstStage
    {
        public int burn_time_sec { get; set; } = 0;
        public int engines { get; set; } = 0;
        public double fuel_amount_tons { get; set; } = 0;
        public bool reusable { get; set; }
        public Force? thrust_sea_level { get; set; }
        public Force? thrust_vaccum { get; set; }
    }
}