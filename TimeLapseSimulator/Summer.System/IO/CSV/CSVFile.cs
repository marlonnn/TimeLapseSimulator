using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Summer.System.IO.CSV
{
    /// <summary>
    /// CSV文件信息类,保存CSV基本文件信息
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-4-8
    /// </remark>
    public static class CsvFileInfo
    {
        /// <summary>
        /// 通常数据间隔字符 缺省为','
        /// </summary>
        public static char BreakSymbol = ',';
    }

    /// <summary>
    /// 读CSV文件类,读取指定的CSV文件
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-4-8
    /// </remark>
    public class CsvFileReader
    {
        /// <summary>
        /// 行链表,CSV文件的每一行就是一个链
        /// </summary>
        private List<List<string>> rowAL;

        /// <summary>
        /// csv文件名
        /// </summary>
        private string fileName;

        /// <summary>
        /// 文件编码
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// 数据间隔字符
        /// </summary>
        private char breakSymbol;

        /// <summary>
        /// 缺省读取CSV文件类构造函数
        /// </summary>
        public CsvFileReader()
        {
            rowAL = new List<List<string>>();
            fileName = string.Empty;
            encoding = Encoding.Default;
            breakSymbol = CsvFileInfo.BreakSymbol;
        }

        /// <summary>
        /// 读取CSV文件类构造函数
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public CsvFileReader(string filename)
            : this(filename, Encoding.Default)
        {
        }

        /// <summary>
        /// 读取CSV文件类构造函数2
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public CsvFileReader(string filename, Encoding encoding)
            : this(filename, encoding, CsvFileInfo.BreakSymbol)
        {
        }

        /// <summary>
        /// 读取CSV文件类构造函数3
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        /// <param name="breakSymbol">数据间隔字符</param>
        public CsvFileReader(string fileName, Encoding encoding, char breakSymbol)
        {
            this.rowAL = new List<List<string>>();
            this.encoding = encoding;
            this.fileName = fileName;
            this.breakSymbol = breakSymbol;
            LoadCsvFile();
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        public int RowCount
        {
            get
            {
                return rowAL.Count;
            }
        }

        /// <summary>
        /// 获取列数
        /// </summary>
        public int ColCount
        {
            get
            {
                int maxCol = 0;
                for (int i = 0; i < this.rowAL.Count; i++)
                {
                    List<string> colAL = this.rowAL[i];

                    maxCol = (maxCol > colAL.Count) ? maxCol : colAL.Count;
                }
                return maxCol;
            }
        }

        /// <summary>
        /// 获取某行某列的数据
        /// row:行,row = 1代表第一行
        /// col:列,col = 1代表第一列  
        /// </summary>
        public string this[int row, int col]
        {
            get
            {
                //数据有效性验证
                CheckRowValid(row);
                CheckColValid(col);
                List<string> colAL = this.rowAL[row - 1];

                //如果请求列数据大于当前行的列时,返回空值
                if (colAL.Count < col)
                {
                    return "";
                }
                return colAL[col - 1];
            }
        }

        /// <summary>
        /// 获取从某行起多行数据
        /// </summary>
        /// <param name="row">起始行号</param>  
        /// <param name="rowcount">行数</param>  
        public List<List<string>> GetData(int row, int rowcount)
        {
            List<List<string>> dataList = new List<List<string>>();
            //数据有效性验证

            for (int i = 0; i < rowcount; i++)
            {
                CheckRowValid(row + i);
                List<string> colAL = this.rowAL[row + i - 1];
                List<string> strList = new List<string>();
                strList.AddRange(colAL);
                dataList.Add(strList);
            }
            return dataList;
        }

        /// <summary>
        /// 检查行数是否是有效的
        /// </summary>
        /// <param name="col"></param>  
        private void CheckRowValid(int row)
        {
            if (row <= 0)
            {
                throw new IndexOutOfRangeException("行数不能小于0");
            }
            if (row > RowCount)
            {
                throw new IndexOutOfRangeException("行数不能大于" + row);
            }
        }

        /// <summary>
        /// 检查列数是否是有效的
        /// </summary>
        /// <param name="col"></param>  
        private void CheckColValid(int col)
        {
            if (col <= 0)
            {
                throw new IndexOutOfRangeException("列数不能小于0");
            }
            if (col > ColCount)
            {
                throw new IndexOutOfRangeException("列数不能小于" + "col");
            }
        }

        /// <summary>
        /// 载入CSV文件
        /// </summary>
        private void LoadCsvFile()
        {
            //对数据的有效性进行验证
            if (string.IsNullOrEmpty(fileName) || !File.Exists(this.fileName))
            {
                throw new FileNotFoundException(fileName);
            }
            else
            {
            }
            if (this.encoding == null)
            {
                this.encoding = Encoding.Default;
            }

            StreamReader sr = new StreamReader(this.fileName, this.encoding);
            string csvDataLine;

            csvDataLine = "";
            while (true)
            {
                string fileDataLine;

                fileDataLine = sr.ReadLine();
                if (fileDataLine == null)
                {
                    break;
                }
                if (csvDataLine == "")
                {
                    csvDataLine = fileDataLine;
                }
                else
                {
                    csvDataLine += "/r/n" + fileDataLine;
                }
                //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                //if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLine(csvDataLine);
                    csvDataLine = "";
                }
            }
            sr.Close();
            //数据行出现奇数个引号
            if (csvDataLine.Length > 0)
            {
                throw new FormatException("CSV文件的格式有错误");
            }
        }

        /// <summary>
        /// 获取两个连续引号变成单个引号的数据行
        /// </summary>
        /// <param name="fileDataLine">文件数据行</param>
        /// <returns></returns>
        private string GetDeleteQuotaDataLine(string fileDataLine)
        {
            return fileDataLine.Replace("\"\"", "\"");
        }

        /// <summary>
        /// 判断字符串是否包含奇数个引号
        /// </summary>
        /// <param name="dataLine">数据行</param>
        /// <returns>为奇数时，返回为真；否则返回为假</returns>
        /*
        private bool IfOddQuota(string dataLine)
        {
            int  quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0;i < dataLine.Length;i++)
            {
                if (dataLine[i] == '\"')
                {
                    quotaCount++;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }   

            return oddQuota;
        }
        */

        /// <summary>
        /// 判断是否以奇数个引号开始
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        /*
        private bool IfOddStartQuota(string dataCell)
        {
            int  quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0;i < dataCell.Length;i++)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }   

            return oddQuota;
        }
        */

        /// <summary>
        /// 判断是否以奇数个引号结尾
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        /*
        private bool IfOddEndQuota(string dataCell)
        {
            int  quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = dataCell.Length -1;i >= 0;i--)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }   

            return oddQuota;
        }
        */

        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLine(string newDataLine)
        {
            List<string> colAL = new List<string>();
            string[] dataArray = newDataLine.Split(breakSymbol);
            //bool  oddStartQuota = false;       //是否以奇数个引号开始
            //string  cellData = "";

            for (int i = 0; i < dataArray.Length; i++)
            {
                /*
                if (oddStartQuota)
                {
                    //因为前面用逗号分割,所以要加上逗号
                    cellData += breakSymbol + dataArray[i];
                    //是否以奇数个引号结尾
                    if (IfOddEndQuota(dataArray[i]))
                    {
                        colAL.Add(GetHandleData(cellData));
                        oddStartQuota = false;
                        continue;
                    }
                }
                else
                */
                {
                    //是否以奇数个引号开始
                    /*
                    if (IfOddStartQuota(dataArray[i]))
                    {
                    //是否以奇数个引号结尾,不能是一个双引号,并且不是奇数个引号

                        if (IfOddEndQuota(dataArray[i]) && dataArray[i].Length > 2 && !IfOddQuota(dataArray[i]))
                        {
                            colAL.Add(GetHandleData(dataArray[i]));
                            oddStartQuota = false;
                            continue;
                        }
                        else
                        {
                            oddStartQuota = true;  
                            cellData = dataArray[i];
                            continue;
                         }
                    }
                    else
                    */
                    {
                        colAL.Add(GetHandleData(dataArray[i]));
                    }
                }
            }
            /*
            if (oddStartQuota)
            //{
            //    throw new FormatException("数据格式有问题");
            }
            */
            rowAL.Add(colAL);
        }

        /// <summary>
        /// 去掉格子的首尾引号，把双引号变成单引号
        /// </summary>
        /// <param name="fileCellData"></param>
        /// <returns></returns>
        private string GetHandleData(string fileCellData)
        {
            if (fileCellData == "")
            {
                return "";
            }
            /*
            if (IfOddStartQuota(fileCellData))
            {
                if (IfOddEndQuota(fileCellData))
                {
                    return fileCellData.Substring(1,fileCellData.Length-2).Replace("\"\"","\""); //去掉首尾引号，然后把双引号变成单引号
                }
                else
                {
                    throw new FormatException("数据引号无法匹配" + fileCellData);
                }    
            }
            else //考虑形如""    """"      """"""   
            {                 
                if (fileCellData.Length >2 && fileCellData[0] == '\"')
                {
                    fileCellData = fileCellData.Substring(1,fileCellData.Length-2).Replace("\"\"","\""); //去掉首尾引号，然后把双引号变成单引号
                }
            }
            */
            return fileCellData;
        }
    }

    /// <summary>
    /// 写CSV文件类,首先给CSV文件赋值,最后通过Save方法进行保存操作
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-4-8
    /// </remark>
    public class CsvFileWriter
    {
        /// <summary>
        /// 行链表,CSV文件的每一行就是一个链
        /// </summary>
        private ArrayList rowAL;

        /// <summary>
        /// CSV文件名
        /// </summary>
        private string fileName;

        /// <summary>
        /// 文件编码
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// 数据间隔字符
        /// </summary>
        private char breakSymbol;


        /// <summary>
        /// 缺省写入CSV文件类构造函数
        /// </summary>
        public CsvFileWriter()
        {
            this.rowAL = new ArrayList();
            this.fileName = "";
            this.encoding = Encoding.Default;
            breakSymbol = CsvFileInfo.BreakSymbol;
        }

        /// <summary>
        /// 写入CSV文件类构造函数1
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public CsvFileWriter(string fileName)
            : this(fileName, Encoding.Default)
        {
        }

        /// <summary>
        /// 写入CSV文件类构造函数2
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public CsvFileWriter(string fileName, Encoding encoding)
            : this(fileName, encoding, CsvFileInfo.BreakSymbol)
        {
        }

        /// <summary>
        /// 写入CSV文件类构造函数3
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        /// <param name="breakSymbol">数据间隔字符</param>
        public CsvFileWriter(string fileName, Encoding encoding, char breakSymbol)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = encoding;
            this.breakSymbol = breakSymbol;
        }

        /// <summary>
        /// row:行,row = 1代表第一行
        /// col:列,col = 1代表第一列
        /// </summary>
        public string this[int row, int col]
        {
            set
            {
                //对行进行判断
                if (row <= 0)
                {
                    throw new IndexOutOfRangeException("行数不能小于0");
                }
                else if (row > this.rowAL.Count) //如果当前列链的行数不够，要补齐
                {
                    for (int i = this.rowAL.Count + 1; i <= row; i++)
                    {
                        this.rowAL.Add(new ArrayList());
                    }
                }
                //对列进行判断
                if (col <= 0)
                {
                    throw new IndexOutOfRangeException("列数不能小于0");
                }
                else
                {
                    ArrayList colTempAL = (ArrayList)this.rowAL[row - 1];

                    //扩大长度
                    if (col > colTempAL.Count)
                    {
                        for (int i = colTempAL.Count + 1; i <= col; i++)
                        {
                            colTempAL.Add("");
                        }
                    }
                    this.rowAL[row - 1] = colTempAL;
                }
                //赋值
                ArrayList colAL = (ArrayList)this.rowAL[row - 1];

                colAL[col - 1] = value;
                this.rowAL[row - 1] = colAL;
            }
        }

        /// <summary>
        /// 获取当前最大行
        /// </summary>
        public int CurMaxRow
        {
            get
            {
                return this.rowAL.Count;
            }
        }

        /// <summary>
        /// 获取最大列
        /// </summary>
        public int CurMaxCol
        {
            get
            {
                int maxCol = 0;
                for (int i = 0; i < this.rowAL.Count; i++)
                {
                    ArrayList colAL = (ArrayList)this.rowAL[i];

                    maxCol = (maxCol > colAL.Count) ? maxCol : colAL.Count;
                }
                return maxCol;
            }
        }

        /// <summary>
        /// 添加表数据到CSV文件中
        /// </summary>
        /// <param name="dataList">表数据</param>
        public void AddData(List<string> dataList)
        {
            List<List<string>> dataLists = new List<List<string>>();
            dataLists.Add(dataList);
            AddData(dataLists);
        }

        /// <summary>
        /// 添加表数据到CSV文件中
        /// </summary>
        /// <param name="dataList">表数据</param>
        public void AddData(List<List<string>> dataList)
        {
            if (dataList == null)
            {
                throw new NullReferenceException("需要添加的表数据为空");
            }

            int curMaxRow = this.rowAL.Count;
            for (int i = 0; i < dataList.Count; i++)
            {
                for (int j = 0; j < dataList[i].Count; j++)
                {
                    this[curMaxRow + i + 1, j + 1] = dataList[i][j];
                }
            }
        }

        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        public void Save()
        {
            //对数据的有效性进行判断
            if (this.fileName == null)
            {
                throw new FileNotFoundException(fileName);
            }
            else if (File.Exists(this.fileName))
            {
                File.Delete(this.fileName);
            }
            if (this.encoding == null)
            {
                this.encoding = Encoding.Default;
            }
            StreamWriter sw = new StreamWriter(this.fileName, false, this.encoding);

            for (int i = 0; i < this.rowAL.Count; i++)
            {
                sw.WriteLine(ConvertToSaveLine((ArrayList)this.rowAL[i]));
            }
            sw.Close();
        }

        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public void Save(string fileName)
        {
            this.fileName = fileName;
            Save();
        }

        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public void Save(string fileName, Encoding encoding)
        {
            this.fileName = fileName;
            this.encoding = encoding;
            Save();
        }

        /// <summary>
        /// 转换成保存行
        /// </summary>
        /// <param name="colAL">一行</param>
        /// <returns></returns>
        private string ConvertToSaveLine(ArrayList colAL)
        {
            string saveLine = "";
            for (int i = 0; i < colAL.Count; i++)
            {
                saveLine += ConvertToSaveCell(colAL[i].ToString());
                //格子间以逗号分割
                if (i < colAL.Count - 1)
                {
                    saveLine += breakSymbol;
                }
            }
            return saveLine;
        }

        /// <summary>
        /// 字符串转换成CSV中的格子
        /// 双引号转换成两个双引号
        /// </summary>
        /// <param name="cell">格子内容</param>
        /// <returns></returns>
        private string ConvertToSaveCell(string cell)
        {
            /*
            cell = cell.Replace("\"", "\"\"");
            if(isNeedAuotation(cell))
            {
                return "\"" + cell + "\"";  
            }
            */
            return cell;
        }

        /// <summary>
        /// 判断是否需要首尾各加一个双引号        
        /// </summary>
        /// <param name="cell">格子内容</param>
        /// <returns></returns>
        private bool isNeedAuotation(string cell)
        {
            if (cell.IndexOf(breakSymbol) != -1)
            {
                return true;
            }
            return false; ;
        }
    }
}
