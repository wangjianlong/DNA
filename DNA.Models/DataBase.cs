﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class DataBase
    {
        /// <summary>
        /// 批准用地面积
        /// </summary>
        public double PZYDMJ { get; set; }
        /// <summary>
        /// 实际用地面积——总面积
        /// </summary>
        public double YDZMJ { get; set; }
        /// <summary>
        /// 实际用地面积——未批准面积
        /// </summary>
        public double WJPZYDMJ { get; set; }
        /// <summary>
        /// 实际用地面积——工业用地
        /// </summary>
        public double GYYD
        {
            get
            {
                return YDZMJ;
            }
        }
        /// <summary>
        /// 建筑总面积
        /// </summary>
        public double JZZMJ { get; set; }
        /// <summary>
        /// 建筑占地面积
        /// </summary>
        public double JZZDMJ { get; set; }
        /// <summary>
        /// 未批准建筑面积
        /// </summary>
        public double WPZJZMJ { get; set; }
        /// <summary>
        /// 未批准建筑占地面积
        /// </summary>
        public double WPZJZZDMJ { get; set; }
        /// <summary>
        /// 土地登记面积
        /// </summary>
        public double TDDJMJ { get; set; }
        /// <summary>
        /// 抵押土地面积
        /// </summary>
        public double DYMJ { get; set; }
        /// <summary>
        /// 承载企业数
        /// </summary>
        public int CZQYSL { get; set; }
        #region 2

        /// <summary>
        /// 高新企业数量
        /// </summary>
        public int SFGXQY { get; set; }
        /// <summary>
        /// 从业人数
        /// </summary>
        public int CYRS { get; set; }
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

        #endregion
        //public static DataBase operator +(DataBase c1, DataBase c2)
        //{
        //    return new DataBase()
        //    {
        //        PZYDMJ = c1.PZYDMJ + c2.PZYDMJ,
        //        YDZMJ = c1.YDZMJ + c2.YDZMJ,
        //        WJPZYDMJ = c1.WJPZYDMJ + c2.WJPZYDMJ,
        //        JZZMJ = c1.JZZMJ + c2.JZZMJ,
        //        JZZDMJ = c1.JZZDMJ + c2.JZZDMJ,
        //        WPZJZMJ = c1.WPZJZMJ + c2.WPZJZMJ,
        //        WPZJZZDMJ = c1.WPZJZZDMJ + c2.WPZJZZDMJ,
        //        TDDJMJ = c1.TDDJMJ + c2.TDDJMJ,
        //        DYMJ = c1.DYMJ + c2.DYMJ,
        //        CZQYSL = c1.CZQYSL + c2.CZQYSL,
        //        SFGXQY = c1.SFGXQY + c2.SFGXQY,
        //        CYRS = c1.CYRS + c2.CYRS,
        //        LJGDZCTZ = c1.LJGDZCTZ + c2.LJGDZCTZ,
        //        YDL2012 = c1.YDL2012 + c2.YDL2012,
        //        YDL2013 = c1.YDL2013 + c2.YDL2013,
        //        YDL2014 = c1.YDL2014 + c2.YDL2014,
        //        GSRKSS2012 = c1.GSRKSS2012 + c2.GSRKSS2012,
        //        GSRKSS2013 = c1.GSRKSS2013 + c2.GSRKSS2013,
        //        GSRKSS2014 = c1.GSRKSS2014 + c2.GSRKSS2014,
        //        DSRKSS2012 = c1.DSRKSS2012 + c2.DSRKSS2012,
        //        DSRKSS2013 = c1.DSRKSS2013 + c2.DSRKSS2013,
        //        DSRKSS2014 = c1.DSRKSS2014 + c2.DSRKSS2014,
        //        ZYYSR2012 = c1.ZYYSR2012 + c2.ZYYSR2012,
        //        ZYYSR2013 = c1.ZYYSR2013 + c2.ZYYSR2013,
        //        ZYYSR2014 = c1.ZYYSR2014 + c2.ZYYSR2014
        //    };
        //}
        public static DataBase operator +(DataBase c1, DataBase c2)
        {
            return new DataBase()
            {
                PZYDMJ = Math.Round(c1.PZYDMJ,2) + Math.Round(c2.PZYDMJ,2),
                YDZMJ = Math.Round(c1.YDZMJ,2) + Math.Round(c2.YDZMJ,2),
                WJPZYDMJ = Math.Round(c1.WJPZYDMJ,2) + Math.Round(c2.WJPZYDMJ,2),
                JZZMJ = Math.Round(c1.JZZMJ,2) + Math.Round(c2.JZZMJ,2),
                JZZDMJ = Math.Round(c1.JZZDMJ,2) + Math.Round(c2.JZZDMJ,2),
                WPZJZMJ = Math.Round(c1.WPZJZMJ,2) + Math.Round(c2.WPZJZMJ,2),
                WPZJZZDMJ = Math.Round(c1.WPZJZZDMJ,2) + Math.Round(c2.WPZJZZDMJ,2),
                TDDJMJ = Math.Round(c1.TDDJMJ,2) + Math.Round(c2.TDDJMJ,2),
                DYMJ = Math.Round(c1.DYMJ,2) + Math.Round(c2.DYMJ,2),
                CZQYSL = c1.CZQYSL + c2.CZQYSL,
                SFGXQY = c1.SFGXQY + c2.SFGXQY,
                CYRS = c1.CYRS + c2.CYRS,
                LJGDZCTZ = Math.Round(c1.LJGDZCTZ,2) + Math.Round(c2.LJGDZCTZ,2),
                YDL2012 = Math.Round(c1.YDL2012,2) + Math.Round(c2.YDL2012,2),
                YDL2013 = Math.Round(c1.YDL2013,2) + Math.Round(c2.YDL2013,2),
                YDL2014 = Math.Round(c1.YDL2014,2) + Math.Round(c2.YDL2014,2),
                GSRKSS2012 = Math.Round(c1.GSRKSS2012,2) + Math.Round(c2.GSRKSS2012,2),
                GSRKSS2013 = Math.Round(c1.GSRKSS2013,2) + Math.Round(c2.GSRKSS2013,2),
                GSRKSS2014 = Math.Round(c1.GSRKSS2014,2) + Math.Round(c2.GSRKSS2014,2),
                DSRKSS2012 = Math.Round(c1.DSRKSS2012,2) + Math.Round(c2.DSRKSS2012,2),
                DSRKSS2013 = Math.Round(c1.DSRKSS2013,2) + Math.Round(c2.DSRKSS2013,2),
                DSRKSS2014 = Math.Round(c1.DSRKSS2014,2) + Math.Round(c2.DSRKSS2014,2),
                ZYYSR2012 = Math.Round(c1.ZYYSR2012,2) + Math.Round(c2.ZYYSR2012,2),
                ZYYSR2013 = Math.Round(c1.ZYYSR2013,2) + Math.Round(c2.ZYYSR2013,2),
                ZYYSR2014 = Math.Round(c1.ZYYSR2014,2) + Math.Round(c2.ZYYSR2014,2)
            };
        }
        /// <summary>
        /// 面积单位换算
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static DataBase operator /(DataBase c1, int a)
        {
            return new DataBase()
            {
                PZYDMJ = c1.PZYDMJ / a,
                YDZMJ = c1.YDZMJ / a,
                WJPZYDMJ = c1.WJPZYDMJ / a,
                JZZMJ = c1.JZZMJ / a,
                JZZDMJ = c1.JZZDMJ / a,
                WPZJZMJ = c1.WPZJZMJ / a,
                WPZJZZDMJ = c1.WPZJZZDMJ / a,
                TDDJMJ = c1.TDDJMJ / a,
                DYMJ = c1.DYMJ / a,
                CZQYSL = c1.CZQYSL,
                SFGXQY = c1.SFGXQY,
                CYRS = c1.CYRS,
                LJGDZCTZ = c1.LJGDZCTZ,
                YDL2012 = c1.YDL2012,
                YDL2013 = c1.YDL2013,
                YDL2014 = c1.YDL2014,
                GSRKSS2012 = c1.GSRKSS2012,
                GSRKSS2013 = c1.GSRKSS2013,
                GSRKSS2014 = c1.GSRKSS2014,
                DSRKSS2012 = c1.DSRKSS2012,
                DSRKSS2013 = c1.DSRKSS2013,
                DSRKSS2014 = c1.DSRKSS2014,
                ZYYSR2012 = c1.ZYYSR2012,
                ZYYSR2013 = c1.ZYYSR2013,
                ZYYSR2014 = c1.ZYYSR2014
            };
        }
    }

    public enum Purpose
    {
        
    }
}
