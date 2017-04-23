using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.Device
{
    /// <summary>
    /// 焦平面
    /// </summary>
    public class Focal
    {
        /// <summary>
        /// 焦平面编码
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 焦平面的图像
        /// </summary>
        public Bitmap Image { get; set; }
    }
}
