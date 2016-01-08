using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class ChangePurpose
    {
        public int Number { get; set; }
        public double SumArea { get; set; }
        public double Area05 { get; set; }
        public double Area07 { get; set; }
        public double Area08 { get; set; }
        public double AreaOther { get; set; }
        public static ChangePurpose operator +(ChangePurpose c1, ChangePurpose c2)
        {
            return new ChangePurpose()
            {
                Number = c1.Number + c2.Number,
                SumArea = c1.SumArea + c2.SumArea,
                Area05 = c1.Area05 + c2.Area05,
                Area07 = c1.Area07 + c2.Area07,
                Area08 = c1.Area08 + c2.Area08,
                AreaOther=c1.AreaOther+c2.AreaOther
            };
        }
        public static ChangePurpose operator /(ChangePurpose c1, int a)
        {
            return new ChangePurpose()
            {
                Number = c1.Number,
                SumArea = c1.SumArea / a,
                Area05 = c1.Area05 / a,
                Area07 = c1.Area07 / a,
                Area08 = c1.Area08 / a,
                AreaOther = c1.AreaOther / a
            };
        }
    }
}
