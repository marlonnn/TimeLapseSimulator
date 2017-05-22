using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.Device
{
    /// <summary>
    /// 胚胎培养系统
    /// </summary>
    public class Device
    {
        /// <summary>
        /// 所有的培养皿
        /// </summary>
        public List<Slide> Slides { get; set; }

        //氧气
        public Oxygen oxygen { get; set; }

        //二氧化碳
        public CarbonDioxide carbonDioxide { get; set; }

        //温度
        public Temperature temperature { get; set; }

        //湿度
        public Humidity humidity { get; set; }

    }
}
