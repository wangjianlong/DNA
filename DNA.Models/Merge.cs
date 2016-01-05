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
                return  (JZRJZB - JZZMJ / YDZMJ) / JZRJZB * YDZMJ/10000;
            }
        }
        /// <summary>
        /// 投资强度潜力
        /// </summary>
        public double TZQDQL
        {
            get
            {
                return SFGSQY ? (TZQDZB * 15 - GDZCYJ / YDZMJ * 10000) / (TZQDZB * 15) * YDZMJ / 10000 : 0;
            }
        }
        public double SSCCQL
        {
            get
            {
                return (SSCCZB * 15 - (DSRKSS2014 + GSRKSS2014) / (YDZMJ / 10000)) / (SSCCZB * 15) * YDZMJ / 10000;
            }
        }
        public double YYSSCCQL
        {
            get
            {
                return SFGSQY ? (ZYYSL * 15 - ZYYSR2014 / (YDZMJ / 10000)) / (ZYYSL * 15) * YDZMJ / 10000 : 0;
            }
        }
    }
}
