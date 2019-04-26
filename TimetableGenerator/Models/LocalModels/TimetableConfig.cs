using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models
{
    public class TimetableConfig
    {
        public User User { get; set; }
        public IEnumerable<TimetableData> TimetableDataList { get; set; }
    }
}
