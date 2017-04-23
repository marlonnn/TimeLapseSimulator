using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.Device
{
    /// <summary>
    /// 培养皿
    /// </summary>
    public class Slide
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 培养皿名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 培养皿所在的位置信息
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 病人信息
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// 所有的胚胎细胞
        /// </summary>
        public List<Cell> Cells { get; set; }
    }
}
