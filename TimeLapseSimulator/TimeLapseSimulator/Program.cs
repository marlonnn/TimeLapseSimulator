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
                SimulatorForm simulatorForm = SpringHelper.GetObject<SimulatorForm>("simulatorForm");
                Application.Run(simulatorForm);

            }
            catch (Exception ee)
            {

            }
        }
    }
}
