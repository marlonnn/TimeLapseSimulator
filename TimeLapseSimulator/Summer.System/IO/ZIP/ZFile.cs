using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.IO.ZIP
{
    /// <summary>
    /// 压缩文件类
    /// 公司：ACEA
    /// 作者：钟文
    /// 创建时间：2016-06-28
    /// </summary>
    public class ZFile
    {
        //包含绝对地址全名的文件
        private string _fileFullName;

        //文件存储的文件夹
        private string _folder;

        //文件大小Byte
        private long _fileLength;
        public ZFile(string fileFullName, string folder, long fileLength)
        {
            this._fileFullName = fileFullName;
            this._folder = folder;
            this._fileLength = fileLength;
        }

        public string FileFullName
        {
            get
            {
                return this._fileFullName;
            }
            set
            {
                this._fileFullName = value;
            }
        }

        public string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        public long FileLength
        {
            get
            {
                return this._fileLength;
            }
            set
            {
                this._fileLength = value;
            }
        }
    }
}
