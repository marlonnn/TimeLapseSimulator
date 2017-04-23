using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.Device
{
    /// <summary>
    /// 病人信息
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 受精时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
