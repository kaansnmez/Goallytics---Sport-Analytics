using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Round : BaseEntity
    {
        public int LeagueApiId { get; set; }
        public int SeasonApiId { get; set; }
        public string RoundApiId { get; set; }
        public List<Fixtures> Fixtures { get; set; }
        public Leagues League { get; set; }
        public Season Season { get; set; }

    }
}
