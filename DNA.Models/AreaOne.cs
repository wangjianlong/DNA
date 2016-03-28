using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public class AreaOne
    {
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
        /// 已开发土地面积
        /// </summary>
        public double YKFTDMJ { get; set; }
        /// <summary>
        /// 未开发土地面积
        /// </summary>
        public double WKFTDMJ { get; set; }
        /// <summary>
        /// 是否位于产业平台
        /// </summary>
        public bool SFWYCYPT { get; set; }
        /// <summary>
        /// 产业平台名称
        /// </summary>
        public string CYPTMC { get; set; }
        /// <summary>
        /// 土地使用情况
        /// </summary>
        public string TDSYQK { get; set; }
        public static AreaOne operator *(AreaOne c1, double a)
        {
            return new AreaOne()
            {
                YDZMJ = c1.YDZMJ * a,
                WJPZYDMJ = c1.WJPZYDMJ * a,
                JZZMJ = c1.JZZMJ * a,
                JZZDMJ = c1.JZZDMJ * a,
                WPZJZMJ = c1.WPZJZMJ * a,
                WPZJZZDMJ = c1.WPZJZZDMJ * a,
                YKFTDMJ=c1.YKFTDMJ*a,
                WKFTDMJ=c1.WKFTDMJ*a,
                SFWYCYPT=c1.SFWYCYPT,
                CYPTMC=c1.CYPTMC,
                TDSYQK=c1.TDSYQK
            };
        }
        public static AreaOne operator /(AreaOne c1, double a)
        {
            return new AreaOne()
            {
                YDZMJ = c1.YDZMJ / a,
                WJPZYDMJ = c1.WJPZYDMJ / a,
                JZZMJ = c1.JZZMJ / a,
                JZZDMJ = c1.JZZDMJ / a,
                WPZJZMJ = c1.WPZJZMJ / a,
                WPZJZZDMJ = c1.WPZJZZDMJ / a,
                YKFTDMJ = c1.YKFTDMJ / a,
                WKFTDMJ = c1.WKFTDMJ / a
            };
        }
    }
}
