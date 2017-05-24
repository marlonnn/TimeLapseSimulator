using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeLapseSimulator.DataBase.ADO;
using TimeLapseSimulator.Device;

namespace TimeLapseSimulator
{
    public class OperationFactory
    {
        private TimeLapseSimulator.Device.Device device;

        private DBOperate dbOperate;

        private bool execute = false;

        public bool Execute
        {
            get { return this.execute; }
            set { this.execute = value; }
        }

        public bool ThreadRun = true;
        public TimeLapseSimulator.Device.Device Device
        {
            set { this.device = value; }
        }

        public DBOperate DBOperate
        {
            set { this.dbOperate = value; }
        }

        public delegate void SetWellColor(int slideID, int row, int col, Color backColor);
        public SetWellColor SetWellColorHandler;

        public delegate void ClearWellColor();
        public ClearWellColor ClearWellColorHandler;

        public delegate void AppendLog(string[] log);
        public AppendLog AppendLogHandler;

        public delegate void Flash(int slideID);
        public Flash FlashHandler;

        private void FlashSlide(int slideID)
        {
            if (FlashHandler != null)
                FlashHandler(slideID);
        }

        public void ExecuteInternal()
        {
            if (device != null)
            {
                List<Slide> Slides = device.Slides;
                if (Slides != null && Slides.Count > 0)
                {
                    //每个培养皿
                    while (Execute)
                    {
                        if (ClearWellColorHandler != null)
                            ClearWellColorHandler();
                        for (int i =0; i< Slides.Count; i++)
                        {
                            InserTEnvironment(i + 1);
                            Slide slide = Slides[i];
                            //TO DO
                            //移动到培养皿对应的坐标位置
                            //...
                            FlashSlide(slide.ID);
                            //闪烁当前Slide

                            List<Cell> cells = slide.Cells;
                            if (cells != null && cells.Count > 0)
                            {
                                //每个胚胎细胞
                                foreach (Cell cell in cells)
                                {
                                    //TO DO 
                                    //移动到每个胚胎细胞对应的坐标位置
                                    //...
                                    if (SetWellColorHandler != null)
                                        SetWellColorHandler(slide.ID,
                                            cell.Position.Row - 1, cell.Position.Column - 1, Color.DarkGreen);
                                    List<Focal> focals = cell.Focals;
                                    if (focals != null && focals.Count > 0)
                                    {
                                        //TO DO
                                        //移动到每个焦平面对应的坐标位置
                                        //...
                                        //每个焦平面
                                        foreach (Focal focal in focals)
                                        {
                                            //1.拍照
                                            byte[] image = Camera.ImageToByteArray(string.Format("{0}\\Images\\default.png", System.Environment.CurrentDirectory));
                                            Thread.Sleep(200);
                                            //2.添加日志信息
                                            if (AppendLogHandler != null)
                                                AppendLogHandler(new string[] {
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ms"),
                                                slide.Name, cell.Name, focal.ID.ToString(),
                                                string.Format("{0}\\Images\\default.png", System.Environment.CurrentDirectory), "Success"});
                                            //3.存数据库
                                            //TSLide s = CreateTSlide(slide, cell, focal, image);
                                            //dbOperate.ExecuteNonQuery(s);
                                            //dbOperate.ExecuteNonQuery(string.Format("Slide{0}", slide.ID), slide.ID, slide.Name, cell.ID, cell.Name, focal.ID, focal.ID.ToString(), image);
                                            Thread.Sleep(100);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private TSLide CreateTSlide(Slide slide, Cell cell, Focal focal, byte[] image)
        {
            TSLide s = new TSLide();
            s.SlideID = slide.ID;
            s.SlideName = slide.Name;
            s.CellID = cell.ID;
            s.CellName = cell.Name;
            s.FocalID = focal.ID;
            s.FocalName = focal.ID.ToString();
            s.Time = DateTime.Now;
            s.Image = image;
            return s;
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;

        }

        private TEnvironment CreateTEnvironment(int cultureID)
        {
            TEnvironment environment = new TEnvironment();
            environment.CultureID = cultureID;
            environment.OxygenConcentration = device.oxygen.Concentration;
            environment.TemperatureValue = device.temperature.Value;
            environment.HumidityValue = device.humidity.Value;
            return environment;
        }

        private void InserTEnvironment(int index)
        {
            try
            {
                var envir = dbOperate.FindTEnvironment(index + 1);
                TEnvironment environment = CreateTEnvironment(index + 1);
                if (envir == null)
                {
                    dbOperate.InsertTEnvironment(environment);
                }
                else
                {
                    dbOperate.UpdateEnvironment(environment);
                }
            }
            catch (Exception ee)
            {

            }
        }
    }
}
