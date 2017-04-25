using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.DataBase.ADO
{
    public class DBOperate
    {
        private SlideADO slideADO;

        public DBOperate(SlideADO slideADO)
        {
            this.slideADO = slideADO;
        }

        public void ExecuteNonQuery(TSLide slide)
        {
            slideADO.Insert(slide);
        }

        public IList<TSLide> QueryAllSlide()
        {
            string sql = "Select * from Slide";
            return slideADO.FindAll(sql);
        }
    }
}
