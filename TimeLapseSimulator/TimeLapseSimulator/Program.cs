using Summer.System.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeLapseSimulator.Device;

namespace TimeLapseSimulator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Cell S1_Cell1 = SpringHelper.GetObject<Cell>("S1_Cell1");

                TimeLapseSimulator.Device.Device device = SpringHelper.GetObject<TimeLapseSimulator.Device.Device>("device");
                Application.Run(new SimulatorForm());

            }
            catch (Exception ee)
            {

            }
        }
    }
}
