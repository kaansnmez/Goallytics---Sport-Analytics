using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class GetLeaguesDto
    {
        public int Id { get; set; }
        public int LeagueApiId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Code { get; set; }
    }
}
