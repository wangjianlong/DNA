using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class DataOne:DataBase
    {
        public DataBase Up { get; set; }
        public DataBase Down { get; set; }
    }
}
