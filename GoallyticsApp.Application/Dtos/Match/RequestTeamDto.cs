using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class RequestTeamDto
    {
        public int TeamApiId { get; set; }
        public string? Name { get; set; }
        public string Code { get; set; }
        public bool? National { get; set; }
    }
}
