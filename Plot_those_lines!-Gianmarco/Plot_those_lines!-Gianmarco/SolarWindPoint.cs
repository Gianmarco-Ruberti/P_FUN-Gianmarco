using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plot_those_lines__Gianmarco
{
    public class SolarWindPoint
    {
        public DateTime TimeTag { get; set; }
        public double? ByGse { get; set; }
        public double? BzGse { get; set; }
        public double? ByGsm { get; set; }
        public double? BzGsm { get; set; }
        public double? Bt { get; set; }
    }
}
