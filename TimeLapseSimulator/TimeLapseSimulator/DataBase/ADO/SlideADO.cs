using Summer.System.Data;
using Summer.System.Data.DbMapping;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLapseSimulator.DataBase.ADO
{
    public class SlideADO : SmrAdoTmplate<TSLide>
    {
    }

    [TableAttribute("Slide")]
    public class TSLide
    {
        [FieldAttribute("Slide_ID")]
        public int SlideID;

        [FieldAttribute("Slide_Name")]
        public string SlideName;

        [FieldAttribute("Cell_ID")]
        public int CellID;

        [FieldAttribute("Cell_Name")]
        public string CellName;

        [FieldAttribute("Focal_ID")]
        public int FocalID;

        [FieldAttribute("Focal_Name")]
        public string FocalName;

        [FieldAttribute("Time")]
        public DateTime Time;

        [FieldAttribute("Image")]
        public Bitmap Image;
    }
}
