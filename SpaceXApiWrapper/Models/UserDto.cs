namespace SpaceXApiWrapper.Models
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
