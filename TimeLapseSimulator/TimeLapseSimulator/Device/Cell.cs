using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.Device
{
    /// <summary>
    /// 胚胎细胞
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// 细胞编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 细胞名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 胚胎细胞在培养皿中的坐标信息
        /// 根据此坐标信息可以计算出实际的坐标信息
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 细胞所有焦平面信息
        /// </summary>
        public List<Focal> Focals { get; set; }
    }
}
