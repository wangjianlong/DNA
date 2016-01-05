﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class MergeBase
    {
        /// <summary>
        /// 是否高新企业
        /// </summary>
        public bool SFGXQY { get; set; }
        /// <summary>
        /// 从业人数
        /// </summary>
        public double CYRS { get; set; }
        /// <summary>
        /// 累计固定资产投资
        /// </summary>
        public double LJGDZCTZ { get; set; }
        /// <summary>
        /// 年用电量 2012
        /// </summary>
        public double YDL2012 { get; set; }
        /// <summary>
        /// 年用电量 2013
        /// </summary>
        public double YDL2013 { get; set; }
        /// <summary>
        /// 年用电量 2014
        /// </summary>
        public double YDL2014 { get; set; }
        /// <summary>
        /// 国税入库税收 2012
        /// </summary>
        public double GSRKSS2012 { get; set; }
        /// <summary>
        /// 国税入库税收 2013
        /// </summary>
        public double GSRKSS2013 { get; set; }
        /// <summary>
        /// 国税入库税收 2014
        /// </summary>
        public double GSRKSS2014 { get; set; }
        /// <summary>
        /// 地税入库税收 2012
        /// </summary>
        public double DSRKSS2012 { get; set; }
        /// <summary>
        /// 地税入库税收 2013
        /// </summary>
        public double DSRKSS2013 { get; set; }
        /// <summary>
        /// 地税入库税收2014
        /// </summary>
        public double DSRKSS2014 { get; set; }
        /// <summary>
        /// 主营业收入 2012
        /// </summary>
        public double ZYYSR2012 { get; set; }
        /// <summary>
        /// 主营业收入 2013
        /// </summary>
        public double ZYYSR2013 { get; set; }
        /// <summary>
        /// 主营业收入 2014
        /// </summary>
        public double ZYYSR2014 { get; set; }
        public double TDZMJ { get; set; }
        public bool SFGSQY { get; set; }
        public static MergeBase operator +(MergeBase c1, MergeBase c2)
        {
            return new MergeBase()
            {
                CYRS = c1.CYRS + c2.CYRS,
                LJGDZCTZ = c1.LJGDZCTZ + c2.LJGDZCTZ,
                YDL2012 = c1.YDL2012 + c2.YDL2012,
                YDL2013 = c1.YDL2013 + c2.YDL2013,
                YDL2014 = c1.YDL2014 + c2.YDL2014,
                GSRKSS2012 = c1.GSRKSS2012 + c2.GSRKSS2012,
                GSRKSS2013 = c1.GSRKSS2013 + c2.GSRKSS2013,
                GSRKSS2014 = c1.GSRKSS2014 + c2.GSRKSS2014,
                DSRKSS2012 = c1.DSRKSS2012 + c2.DSRKSS2012,
                DSRKSS2013 = c1.DSRKSS2013 + c2.DSRKSS2013,
                DSRKSS2014 = c1.DSRKSS2014 + c2.DSRKSS2014,
                ZYYSR2012 = c1.ZYYSR2012 + c2.ZYYSR2012,
                ZYYSR2013 = c1.ZYYSR2013 + c2.ZYYSR2013,
                ZYYSR2014 = c1.ZYYSR2014 + c2.ZYYSR2014,
                TDZMJ=c1.TDZMJ+c2.TDZMJ
            };
        }
        public static MergeBase operator -(MergeBase c1, MergeBase c2)
        {
            return new MergeBase()
            {
                CYRS = c1.CYRS - c2.CYRS,
                LJGDZCTZ = c1.LJGDZCTZ - c2.LJGDZCTZ,
                YDL2012 = c1.YDL2012 - c2.YDL2012,
                YDL2013 = c1.YDL2013 - c2.YDL2013,
                YDL2014 = c1.YDL2014 = c2.YDL2014,
                GSRKSS2012 = c1.GSRKSS2012 - c2.GSRKSS2012,
                GSRKSS2013 = c1.GSRKSS2013 - c2.GSRKSS2013,
                GSRKSS2014 = c1.GSRKSS2014 - c2.GSRKSS2014,
                DSRKSS2012 = c1.DSRKSS2012 - c2.DSRKSS2012,
                DSRKSS2013 = c1.DSRKSS2013 - c2.DSRKSS2013,
                DSRKSS2014 = c1.DSRKSS2014 - c2.DSRKSS2014,
                ZYYSR2012 = c1.ZYYSR2012 - c2.ZYYSR2012,
                ZYYSR2013 = c1.ZYYSR2013 - c2.ZYYSR2013,
                ZYYSR2014 = c1.ZYYSR2014 - c2.ZYYSR2014,
                TDZMJ = c1.TDZMJ - c2.TDZMJ
            };
        }
        public static MergeBase operator *(MergeBase c1, double a)
        {
            return new MergeBase()
            {
                CYRS = c1.CYRS * a,
                LJGDZCTZ = c1.LJGDZCTZ * a,
                YDL2012 = c1.YDL2012 * a,
                YDL2013 = c1.YDL2013 * a,
                YDL2014 = c1.YDL2014 * a,
                GSRKSS2012 = c1.GSRKSS2012 * a,
                GSRKSS2013 = c1.GSRKSS2013 * a,
                GSRKSS2014 = c1.GSRKSS2014 * a,
                DSRKSS2012 = c1.DSRKSS2012 * a,
                DSRKSS2013 = c1.DSRKSS2013 * a,
                DSRKSS2014 = c1.DSRKSS2014 * a,
                ZYYSR2012 = c1.ZYYSR2012 * a,
                ZYYSR2013 = c1.ZYYSR2013 * a,
                ZYYSR2014 = c1.ZYYSR2014 * a,
                TDZMJ=c1.TDZMJ*a
            };
        }
    }
    public class TempData
    {
        public TempData()
        {
            Reduce = new MergeBase();
        }
        public double Sum { get; set; }
        public int Count { get; set; }
        public MergeBase merge { get; set; }
        public MergeBase Reduce { get; set; }
    }
}
