using LockedCounter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockedCounter.Storage
{
    public class StateDurationRepository : BaseRepository<StateDuration>
    {
        public StateDurationRepository() : base("stateDurations.json") { }
        public StateDurationRepository(IList<StateDuration> initialData) //Tests
        {
            _collection = initialData;
        }
    }
}
