using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.AuthEntities
{
    public class AppUserRoles : BaseEntity
    {
        public int AppUserId { get; set; }
        public int AppRoleId { get; set; }
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}
