using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Team : BaseEntity
    {
        public int TeamApiId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string? Founded { get; set; }
        public bool? National { get; set; }
        public string Logo { get; set; }
        public ICollection<Fixtures> HomeFixtures { get; set; } = new List<Fixtures>();
        public ICollection<Fixtures> AwayFixtures { get; set; } = new List<Fixtures>();

    }
}
