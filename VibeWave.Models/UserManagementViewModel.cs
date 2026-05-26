using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VibeWave.Models
{
    public class UserManagementViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> CurrentRoles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
    }
}