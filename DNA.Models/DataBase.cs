using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class DataBase
    {
        public bool Scope { get; set; }
        public double Approve { get; set; }
        public LandUse landUse { get; set; }
        public double BuildSumArea { get; set; }
        public double BuildArea { get; set; }
        public double WPZJZMJ { get; set; }
        public int CYRS { get; set; }
        public double LJGDZCTZ { get; set; }

        public double[] Electricity { get; set; }
        public double[] CentralTax { get; set; }
        public double[] FarmTax { get; set; }
        public double[] MainBusiness { get; set; }
    }
}
