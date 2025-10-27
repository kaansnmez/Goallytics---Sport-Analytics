using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Leagues : BaseEntity
    {
        public int LeagueApiId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public ICollection<Fixtures> Fixtures { get; set; }
        public ICollection<Round> Rounds { get; set; }


    }
}
