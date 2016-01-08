using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class Parcel
    {
        public int Number { get; set; }
        public double Area { get; set; }
        public static Parcel operator +(Parcel c1, Parcel c2)
        {
            return new Parcel()
            {
                Number = c1.Number + c2.Number,
                Area = c1.Area + c2.Area
            };
        }
        public static Parcel operator /(Parcel c1, int a)
        {
            return new Parcel()
            {
                Number = c1.Number,
                Area = c1.Area / a
            };
        }
    }
}
