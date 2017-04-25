using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace Summer.System.IO
{
    /// <summary>
    /// INI文件读写类。
    /// INI首层为段，各个段内包含键值对。
    /// 此类包含的操作有：
    /// （1）创建实例
    /// CreateFromFile——从ini文件创建实例；
    /// （2）写键值
    /// WriteValue——如果不存在键，则新建；如果存在，则更新；
    /// （3）读section列表
    /// ReadAllSectionNames——获得所有section；
    /// （4）读值
    /// ReadKeyValues——获得某个section下所有的键值组合；
    /// ReadStringValue——读取某个section某个key的字符串值；
    /// ReadBytesValue——读取某个section某个key的字节值；
    /// （5）删除
    /// RemoveAllSection——删除所有段落；
    /// RemoveSection——删除某个section；
    /// </summary>
    /// <remark>
    /// 思路：调用WritePrivateProfileString和GetPrivateProfileString处理INI文件
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-4-8
    /// </remark>
    public class IniFileHelper
    {
        /// <summary>
        /// INI 文件绝对路径
        /// </summary>
        public string Path { get; protected set; }

        protected IniFileHelper(string filePath)
		{
			Path = filePath;
		}

        /// <summary>
        /// 获取指定文件的INIFILE对象
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>INIFiled对象</returns>
        public static IniFileHelper CreateFromFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                return new IniFileHelper(filePath);
            }
            return null;
        }

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def, StringBuilder retVal,int size,string filePath);

	
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);

		/// <summary>
		/// 写INI文件
		/// </summary>
        /// <param name="section">Section名称</param>
        /// <param name="key">Key名称</param>
        /// <param name="value">Value字符串</param>
        /// <returns>true：执行成功，false：执行失败</returns>
        public bool WriteValue(string section, string key, string value)
		{
			return WritePrivateProfileString(section,key,value,Path) != 0;
		}

        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadAllSectionNames()
        {
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, Path);
            if (bytesReturned == 0)
            {
                return null;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            string[] sections = local.Substring(0, local.Length - 1).Split('\0');
            return sections;
        }

        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReadKeyValues(string section)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            byte[] b = new byte[65535];

            GetPrivateProfileSection(section, b, b.Length, Path);
            
            string s = Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            ArrayList result = new ArrayList();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    list[ item[0].Trim() ] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    list[ item[0].Trim() ] = "";
                }
                else if (item.Length == 0)
                {
                }
            }

            return list;
        }

		/// <summary>
		/// 读取某个字符串键值（注意，字符串长度最大255个字符）
		/// </summary>
		/// <param name="section">Section名称</param>
		/// <param name="key">Key名称</param>
		/// <returns>Value字符串</returns>
		public string ReadStringValue(string section,string key)
		{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(section,key,"",temp, 255, Path);
			return temp.ToString();
		}

        /// <summary>
        /// 读取byte数组键值（注意，最大255个字节）
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
		public byte[] ReadBytesValue(string section, string key)
		{
			byte[] temp = new byte[255];
			int i = GetPrivateProfileString(section, key, "", temp, 255, Path);
			return temp;
		}

		/// <summary>
		/// 删除INI文件下所有段落
		/// </summary>
        /// <returns>true：执行成功，false：执行失败</returns>
        public bool RemoveAllSection()
		{
			return WriteValue(null,null,null);
		}

		/// <summary>
		/// 删除ini文件下指定段落下的所有键
		/// </summary>
        /// <param name="section">Section名称</param>
        /// <returns>true：执行成功，false：执行失败</returns>
        public bool RemoveSection(string section)
		{
            return WriteValue(section, null, null);
		}
    }
}
