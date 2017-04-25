using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Summer.System.IO
{
    public class ExcelReaderHelper
    {
        private HSSFWorkbook mWorkBook;

        /// <summary>
        /// 构造函数，根据指定路径读取Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public ExcelReaderHelper(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            using(FileStream fileStream=new FileStream(filePath,FileMode.Open,FileAccess.Read))
            {
                mWorkBook = new HSSFWorkbook(fileStream);
            }
        }

        /// <summary>
        /// 获取指定sheet的数据
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="cellsCount">指定列的个数</param>
        /// <returns>返回指定sheet指定列的数据</returns>
        public DataTable GetSheet(string sheetName,int cellsCount)
        {
            ISheet sheet = mWorkBook.GetSheet(sheetName);
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < cellsCount; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }
            while(rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for(int i=0;i<row.LastCellNum;i++)
                {
                    ICell cell = row.GetCell(i);
                    if(cell==null)
                    {
                        dr[i]=null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
