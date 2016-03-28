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
        public HY Hy { get; set; }
        public bool SFGSQY { get; set; }
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

    public class HY
    {
        public string HYLB { get; set; }
        public string HYDM { get; set; }
    }
    public class PotentialBase
    {
        public double JZRJQL { get; set; }
        public double TZQDQL { get; set; }
        public double SSCCQL { get; set; }
        public double YYSSCCQL { get; set; }
        public static PotentialBase operator +(PotentialBase c1, PotentialBase c2)
        {
            return new PotentialBase()
            {
                JZRJQL = c1.JZRJQL + c2.JZRJQL,
                TZQDQL = c1.TZQDQL + c2.TZQDQL,
                SSCCQL = c1.SSCCQL + c2.SSCCQL,
                YYSSCCQL = c1.YYSSCCQL + c2.YYSSCCQL
            };
        }
    }

    public class Potential
    {
        /// <summary>
        /// 建筑总面积
        /// </summary>
        public double JZZMJ { get; set; }
        /// <summary>
        /// 用地总面积
        /// </summary>
        public double YDZMJ { get; set; }
        /// <summary>
        /// 固定资产原价
        /// </summary>
        public double GDZCYJ { get; set; }
        /// <summary>
        /// 国税入库税收2014
        /// </summary>
        public double GSRKSS2014 { get; set; }
        /// <summary>
        /// 地税入库税收2014
        /// </summary>
        public double DSRKSS2014 { get; set; }
        public double ZYYSR2014 { get; set; }
        /// <summary>
        /// 建筑容积指标
        /// </summary>
        public double JZRJZB { get; set; }
        /// <summary>
        /// 投资强度指标
        /// </summary>
        public double TZQDZB { get; set; }
        /// <summary>
        /// 税收产出指标
        /// </summary>
        public double SSCCZB { get; set; }
        /// <summary>
        /// 主营业收入指标
        /// </summary>
        public double ZYYSL { get; set; }
        /// <summary>
        /// 累计固定资产投资
        /// </summary>
        public double LJGDZCTZ { get; set; }
        /// <summary>
        /// 是否规上
        /// </summary>
        public bool SFGSQY { get; set; }
        public string DKBH { get; set; }
        /// <summary>
        /// 建筑容积潜力
        /// </summary>
        public double JZRJQL
        {
            get
            {
                var val= (JZRJZB - JZZMJ / YDZMJ) / JZRJZB * YDZMJ / 10000;
                return val < 0 ? 0 : val;
            }
        }
        /// <summary>
        /// 投资强度潜力
        /// </summary>
        public double TZQDQL
        {
            get
            {
                var val = (TZQDZB * 15 - LJGDZCTZ / YDZMJ * 10000) / (TZQDZB * 15) * YDZMJ / 10000;
                return SFGSQY ? (val < 0 ? 0 : val) : 0;
            }
        }
        public double SSCCQL
        {
            get
            {
                var val= (SSCCZB * 15 - (DSRKSS2014 + GSRKSS2014) / (YDZMJ / 10000)) / (SSCCZB * 15) * YDZMJ / 10000;
                return val < 0 ? 0 : val;
            }
        }
        public double YYSSCCQL
        {
            get
            {
                var val = (ZYYSL * 15 - ZYYSR2014 / (YDZMJ / 10000)) / (ZYYSL * 15) * YDZMJ / 10000;
                return SFGSQY ? (val < 0 ? 0 : val) : 0;
            }
        }
    }

    public class PotentialFive
    {
        public PotentialBase Up { get; set; }
        public PotentialBase Down { get; set; }
        public static PotentialFive operator +(PotentialFive c1, PotentialFive c2)
        {
            return new PotentialFive()
            {
                Up = c1.Up + c2.Up,
                Down = c1.Down + c2.Down
            };
        }
    }
    public enum SheetEnum
    {
        one,
        two,
        three,
        four,
        five,
        six
    }
}
