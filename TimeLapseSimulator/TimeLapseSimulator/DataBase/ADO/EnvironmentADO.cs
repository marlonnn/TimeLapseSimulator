using Summer.System.Data;
using Summer.System.Data.DbMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.DataBase.ADO
{
    public class EnvironmentADO : SmrAdoTmplate<TEnvironment>
    {

    }

    [TableAttribute("Environment")]
    public class TEnvironment
    {
        //培养仓编号
        [FieldAttribute("Culture_ID", PrimaryKey = true)]
        public int CultureID;

        //氧气浓度
        [FieldAttribute("Oxygen_Concentration")]
        public float OxygenConcentration;

        //温度
        [FieldAttribute("Temperature_Value")]
        public float TemperatureValue;

        //湿度
        [FieldAttribute("Humidity_Value")]
        public float HumidityValue;
    }
}
