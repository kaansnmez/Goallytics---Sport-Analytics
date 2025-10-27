using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.AuthEntities
{
    public class AppRole : BaseEntity
    {
        public string Definition { get; set; }
        public List<AppUserRoles> AppUserRoles { get; set; }

    }
}
