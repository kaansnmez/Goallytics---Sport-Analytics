using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.AuthEntities
{
    public class AppUser : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  
        public DateTime DateOfBirth { get; set; }
        public int AppRoleId { get; set; } 
        public List<AppUserRoles> AppUserRoles { get; set; } // Örnek roller: "Admin", "User"
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
    }
}
