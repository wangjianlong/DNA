using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class Merge
    {
        public string DKBH { get; set; }

        /// <summary>
        /// 承载企业数
        /// </summary>
        public int CZQYSL { get; set; }
        /// <summary>
        /// 高新企业数量
        /// </summary>
        public int SFGXQY { get; set; }
        public MergeBase Base { get; set; }
        public static Merge operator +(Merge c1, Merge c2)
        {
            return new Merge()
            {
                CZQYSL = c1.CZQYSL + c2.CZQYSL,
                SFGXQY = c1.SFGXQY + c2.SFGXQY,
                Base = c1.Base + c2.Base
            };
        }
    }
}
