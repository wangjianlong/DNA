using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class AParcel
    {
        public Parcel WKF { get; set; }
        public Parcel BFWKF { get; set; }
        public Parcel HE { get; set; }
        public Parcel Sum
        {
            get
            {
                return WKF + BFWKF;
            }
        }
        public static AParcel operator +(AParcel c1, AParcel c2)
        {
            return new AParcel()
            {
                WKF = c1.WKF + c2.WKF,
                BFWKF = c1.BFWKF + c2.BFWKF,
                HE = c1.HE + c2.HE
            };
        }
    }
}
