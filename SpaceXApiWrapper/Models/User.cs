using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SpaceXApiWrapper.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
