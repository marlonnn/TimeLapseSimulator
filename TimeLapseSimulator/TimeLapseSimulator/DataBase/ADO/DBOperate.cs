using MySql.Data.MySqlClient;
using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TimeLapseSimulator.DataBase.ADO
{
    public class DBOperate
    {
        private SlideADO slideADO;

        private IDbProvider provider;
        private AdoTemplate adoTemplate;

        public void ExecuteNonQuery(TSLide slide, string tableName)
        {
            string sql = string.Format("insert into {0}(Slide_ID, Slide_Name, Cell_ID, Cell_Name, Focal_ID, Focal_Name, Time, Image) values(@Slide_ID, @Slide_Name, @Cell_ID, @Cell_Name, @Focal_ID, @Focal_Name, @Time, @Image)", tableName);
            DbParameters parameters = new DbParameters(provider);
            parameters.AddParameter(new SqlParameter("@Slide_ID", slide.SlideID));
            parameters.AddParameter(new SqlParameter("@Slide_Name", slide.SlideName));
            parameters.AddParameter(new SqlParameter("@Cell_ID", slide.CellID));
            parameters.AddParameter(new SqlParameter("@Cell_Name", slide.CellName));
            parameters.AddParameter(new SqlParameter("@Focal_ID", slide.FocalID));
            parameters.AddParameter(new SqlParameter("@Focal_Name", slide.FocalName));
            parameters.AddParameter(new SqlParameter("@Time", DateTime.Now));
            parameters.AddParameter(new SqlParameter("@Image", slide.Image));
            adoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, sql, parameters);
        }

        public void ExecuteNonQuery(string tableName, int SlideID,string SlideName, int CellID, string CellName, int FocalID, string FocalName, byte[] Image)
        {
            string sql = string.Format("insert into {0}(Slide_ID, Slide_Name, Cell_ID, Cell_Name, Focal_ID, Focal_Name, Time, Image) values(@Slide_ID, @Slide_Name, @Cell_ID, @Cell_Name, @Focal_ID, @Focal_Name, @Time, @Image)", tableName);
            DbParameters parameters = new DbParameters(provider);
            parameters.AddParameter(new MySqlParameter("@Slide_ID", SlideID));
            parameters.AddParameter(new MySqlParameter("@Slide_Name", SlideName));
            parameters.AddParameter(new MySqlParameter("@Cell_ID", CellID));
            parameters.AddParameter(new MySqlParameter("@Cell_Name", CellName));
            parameters.AddParameter(new MySqlParameter("@Focal_ID", FocalID));
            parameters.AddParameter(new MySqlParameter("@Focal_Name", FocalName));
            parameters.AddParameter(new MySqlParameter("@Time", DateTime.Now));
            parameters.AddParameter(new MySqlParameter("@Image", Image));
            adoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, sql, parameters);
        }

        //public DBOperate(SlideADO slideADO)
        //{
        //    this.slideADO = slideADO;
        //}

        public void ExecuteNonQuery(TSLide slide)
        {
            slideADO.Insert(slide);
        }

        public IList<TSLide> QueryAllSlide(string tableName, int cellID, int focalID)
        {
            string sql = string.Format("Select * from {0} where Cell_ID = {1} and Focal_ID = {2}", tableName, cellID, focalID);
            return slideADO.FindAll(sql);
        }

        public IList<TSLide> QuerySlides(string tableName, int currentIndex)
        {
            string sql = string.Format("Select * from {0} where ID > {1} and ID <= {2}", tableName, currentIndex, currentIndex + 10);
            return slideADO.FindAll(sql);
        }
    }
}
