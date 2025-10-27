using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Season: BaseEntity
    {
        public int SeasonApiId { get; set; }
        public int SeasonYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Fixtures> Fixtures { get; set; }
        public ICollection<Round> Rounds { get; set; }

    }
}
