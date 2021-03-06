﻿using MySql.Data.MySqlClient;
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
        private EnvironmentADO environmentADO;
        private IDbProvider provider;
        private AdoTemplate adoTemplate;
        public string queryMode;//Binary or ImagePath
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

        public void ExecuteNonQuery(string tableName, int SlideID,string SlideName, int CellID, string CellName, int FocalID, string FocalName, byte[] Image, string imagePath)
        {
            string sql = string.Format("insert into {0}(Slide_ID, Slide_Name, Cell_ID, Cell_Name, Focal_ID, Focal_Name, Time, Image, Image_Path) values(@Slide_ID, @Slide_Name, @Cell_ID, @Cell_Name, @Focal_ID, @Focal_Name, @Time, @Image, @Image_Path)", tableName);
            DbParameters parameters = new DbParameters(provider);
            parameters.AddParameter(new MySqlParameter("@Slide_ID", SlideID));
            parameters.AddParameter(new MySqlParameter("@Slide_Name", SlideName));
            parameters.AddParameter(new MySqlParameter("@Cell_ID", CellID));
            parameters.AddParameter(new MySqlParameter("@Cell_Name", CellName));
            parameters.AddParameter(new MySqlParameter("@Focal_ID", FocalID));
            parameters.AddParameter(new MySqlParameter("@Focal_Name", FocalName));
            parameters.AddParameter(new MySqlParameter("@Time", DateTime.Now));
            parameters.AddParameter(new MySqlParameter("@Image", Image));
            parameters.AddParameter(new MySqlParameter("@Image_Path", imagePath));
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

        public void UpdateEnvironment(TEnvironment environment)
        {
            environmentADO.Update(environment);
        }

        public TEnvironment FindTEnvironment(int cultureID)
        {
            string sql = string.Format("select * from environment where Culture_ID = {0}", cultureID);
            return environmentADO.Find(sql);
        }

        public void InsertTEnvironment(TEnvironment environment)
        {
            environmentADO.Insert(environment);
        }

        public IList<TSLide> QueryAllSlide(string tableName, int cellID, int focalID)
        {
            string sql = string.Format("Select * from {0} where Cell_ID = {1} and Focal_ID = {2}", tableName, cellID, focalID);
            return slideADO.FindAll(sql);
        }

        public IList<TSLide> QuerySlides(string tableName, int cellID, int focusID)
        {
            IList<TSLide> slides = null;
            switch (queryMode)
            {
                case "Binary":
                    string sql = string.Format("Select * from {0} where Cell_ID = {1} and Focal_ID = {2}", tableName, cellID, focusID);
                    slides = slideADO.FindAll(sql);
                    break;
                case "ImagePath":
                    slides = new List<TSLide>();
                    string sql1 = string.Format("Select ID, Slide_ID, Slide_Name, Cell_ID, Cell_Name,Focal_ID,Focal_Name,Time,Image_Path from {0} where (Cell_ID = {1} and Focal_ID = {2})", tableName, cellID, focusID);
                    adoTemplate.QueryWithRowCallbackDelegate(System.Data.CommandType.Text,
                        sql1, 
                        (r) =>
                        {
                            try
                            {
                                TSLide slide = new TSLide();
                                int ID = r.GetInt32(0);
                                slide.SlideID = r.GetInt32(1);
                                slide.SlideName = r.GetString(2);
                                slide.CellID = r.GetInt32(3);
                                slide.CellName = r.GetString(4);
                                slide.FocalID = r.GetInt32(5);
                                slide.FocalName = r.GetString(6);
                                slide.Time = r.GetDateTime(7);
                                slide.ImagePath = r.GetString(8);
                                slides.Add(slide);
                            }
                            catch (Exception ee)
                            {

                            }
                        });
                    break;
            }
            return slides;
        }
    }
}
