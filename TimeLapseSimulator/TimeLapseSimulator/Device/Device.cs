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
    }
}
