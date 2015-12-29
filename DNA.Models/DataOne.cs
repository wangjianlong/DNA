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
        public static DataOne operator +(DataOne c1, DataOne c2)
        {
            return new DataOne()
            {
                Up = c1.Up + c2.Up,
                Down = c1.Down + c2.Down
            };
        }
    }
}
