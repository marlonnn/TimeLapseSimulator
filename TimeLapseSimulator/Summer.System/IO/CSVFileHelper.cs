using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.IO.CSV;

namespace Summer.System.IO
{
    /// <summary>
    /// XML解析处理类，含对XML文件和XML字符串的解析和操作。
    /// 通过此类载入xml数据，得到root节点后，就可以通过操作XmlVisitor对象来操作所有的节点。
    /// 此类包含的操作有：
    /// （1）静态初始化方法：
    /// CreateFromFile——从文件创建；
    /// CreateFromList——从二维数组创建；
    /// （2）读取函数：
    /// ReadAll——返回所有数据；
    /// GetRowsCount——得到总行数；
    /// GetColsCount——得到指定行的列数；
    /// GetMaxColsCount——得到最大的列数；
    /// ReadRow——返回某行数据；
    /// ReadCol——返回某列数据；
    /// ReadCell——返回某个单元格的数据；
    /// （3）新增操作：
    /// Insert——在指定行号插入数据；
    /// Append——在最后一行追加数据；
    /// （4）删除操作：
    /// RemoveAll——移除所有数据；
    /// RemoveRow——移除某一行；
    /// RemoveCol——移除某一列；
    /// （5）修改操作：
    /// Update——修改某个单元格数据；
    /// （6）保存操作：
    /// Save2File——保存到文件；
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-4-8
    /// </remark>
    public class CSVFileHelper
    {
        /// <summary>
        /// 从文件创建CSV对象实例，注意默认数据的增删改只对内存操作，如果需要保存，最后需要手动调用save函数，或者将sync2File打开，但是效率不高，请调用者自己权衡。
        /// </summary>
        /// <param name="fileName">文件名，此文件必须存在</param>
        /// <param name="encoding">编码格式，默认可以使用 Encoding.Default</param>
        /// <param name="breakSymbol">分隔符，默认使用英文逗号</param>
        /// <param name="sync2File">是否将更改实时同步到文件中，默认关闭。</param>
        /// <returns></returns>
        public static CSVFileHelper CreateFromFile(string fileName, Encoding encoding, char breakSymbol = ',', bool sync2File = false)
        {
            CSVFileHelper helper = new CSVFileHelper();
            helper.csvFilename = fileName;
            helper.csvEncoding = encoding;
            helper.csvBreakSymbol = breakSymbol;
            helper.csvFileReader = new CsvFileReader(fileName, encoding, breakSymbol);
            //从文件载入完成后，以后所有的操作都只针对内存操作，除非调用save函数保存；
            helper.Load();

            return helper;
        }

        /// <summary>
        /// 从一个二维数组创建一个csv格式数据
        /// </summary>
        /// <param name="datalist"></param>
        /// <returns></returns>
        public static CSVFileHelper CreateFromList(List<List<string>> datalist)
        {
            CSVFileHelper helper = new CSVFileHelper();
            helper.data = datalist;
            helper.csvSync2File = false;
            return helper;
        }

        /// <summary>
        /// 返回CSV的所有数据
        /// </summary>
        /// <returns></returns>
        public List<List<string>> ReadAll()
        {
            return data;
        }
        /// <summary>
        /// 返回总行数
        /// </summary>
        /// <returns></returns>
        public int GetRowsCount()
        {
            return data.Count;
        }
        /// <summary>
        /// 返回最大的列数（每一行列数可能不同）
        /// </summary>
        /// <returns></returns>
        public int GetMaxColsCount()
        {
            int maxCols = 0;
            foreach (var row in data)
            {
                if (row.Count > maxCols)
                {
                    maxCols = row.Count;
                }
            }
            return maxCols;
        }
        /// <summary>
        /// 返回指定行的列数；
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public int GetColsCount(int rowNum)
        {
            return data[rowNum].Count;
        }
        /// <summary>
        /// 读取某一行，行号从0开始计数，最后一行为GetRowsCount()-1
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public List<string> ReadRow(int rowNum)
        {
            return data[rowNum];
        }
        /// <summary>
        /// 读取某一列数据，列号从0开始计数，最后一列为GetColsCount()-1.
        /// 由于每行列数不同，因此如果某行没有此列，则插入null，以此标识。
        /// </summary>
        /// <param name="colNum"></param>
        /// <returns></returns>
        public List<string> ReadCol(int colNum)
        {
            List<string> res = new List<string>(GetRowsCount());
            foreach (var row in data)
            {
                if (row.Count > colNum)
                {
                    res.Add(row[colNum]);
                }
                else
                {
                    res.Add(null);
                }
            }
            return res;
        }
        /// <summary>
        /// 读取某个单元格的数据，行号、列号均从0开始计数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <returns></returns>
        public string ReadCell(int rowNum, int colNum)
        {
            if (data.Count > rowNum)
            {
                if (data[rowNum].Count > colNum)
                {
                    return data[rowNum][colNum];
                }
            }
            return null;
        }
        /// <summary>
        /// 在当前行号上插入一条记录，行号从0开始计数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="rowData"></param>
        public void Insert(int rowNum, List<string> rowData)
        {
            data.Insert(rowNum, rowData);
            Flush2File();
        }
        /// <summary>
        /// 在最后追加一行数据
        /// </summary>
        /// <param name="rowData"></param>
        public void Append(List<string> rowData)
        {
            data.Add(rowData);
            Flush2File();
        }
        /// <summary>
        /// 更新某个单元格的值，行号、列号从0开始计数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="value"></param>
        public void UpdateCell(int rowNum, int colNum, string value)
        {
            data[rowNum][colNum] = value;
            Flush2File();
        }
        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void RemoveAll()
        {
            data.Clear();
            Flush2File();
        }
        /// <summary>
        /// 移除某一行，行号从0开始计数
        /// </summary>
        /// <param name="rowNum"></param>
        public void RemoveRow(int rowNum)
        {
            data.RemoveAt(rowNum);
            Flush2File();
        }
        /// <summary>
        /// 移除某一列，列号从0开始计数
        /// </summary>
        /// <param name="colNum"></param>
        public void ReomveCol(int colNum)
        {
            foreach (var row in data)
            {
                row.RemoveAt(colNum);
            }
            Flush2File();
        }
        /// <summary>
        /// 保存到文件（对于从文件创建实例的有效）
        /// </summary>
        public void Save2File()
        {
            Save2File(csvFilename, csvEncoding, csvBreakSymbol);
        }
        /// <summary>
        /// 将数据另存为，如果文件已经存在，则覆盖。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding">null表示使用默认编码方式</param>
        /// <param name="breakSymbol"></param>
        public void Save2File(string fileName, Encoding encoding, char breakSymbol)
        {
            CsvFileWriter csvFileWriter = new CsvFileWriter(fileName, encoding, breakSymbol);
            csvFileWriter.AddData(data);
            csvFileWriter.Save();
        }

        //文件名
        private string csvFilename;
        //读CSV文件类实例
        private CsvFileReader csvFileReader;
        //编码格式
        private Encoding csvEncoding;
        //分隔符号
        private char csvBreakSymbol;
        //是否将更改实时同步到文件中标志
        private bool csvSync2File;
        //csv数据内容存储区域
        private List<List<string>> data = new List<List<string>>();

        /// <summary>
        /// 从文件读取数据
        /// </summary>
        private void Load()
        {
            //读取时，第一行为1，第一列也为1
            data = csvFileReader.GetData(1, csvFileReader.RowCount);
        }
        private void Flush2File()
        {
            if (csvSync2File)
            {
                Save2File(csvFilename, csvEncoding, csvBreakSymbol);
            }
        }
    }
}
