using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Summer.System.Util;

namespace Summer.System.Security
{
    /// <summary>
    /// DES算法加密解密实现类，均为静态函数
    /// </summary>
    /// <remark>
    /// 思路：封装.Net的DESCryptoServiceProvider
    /// 公司：CASCO
    /// 作者：张广宇
    /// 创建日期：2013-5-16
    /// </remark>
    static public class DESHelper
    {
        /// <summary>
        /// DES加密函数,调用时需处理异常
        /// </summary>
        /// <param name="code">需要加密的字符串</param>
        /// <param name="key">长度必须是8位</param>
        /// <returns></returns>
        static public string Encrypt(string code, string key)
        {
            byte[] inputByteArray = ByteHelper.String2Byte(code);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ByteHelper.String2Byte(key);
            des.IV = ByteHelper.String2Byte(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return ByteHelper.Byte2Xstring(ms.ToArray());
        }

        /// <summary>
        /// DES解密函数,调用时需处理异常
        /// </summary>
        /// <param name="code">需要解密的字符串</param>
        /// <param name="key">长度必须是8位,且与加密时Key相同</param>
        /// <returns></returns>
        static public string Decrypt(string code, string key)
        {
            byte[] inputByteArray = ByteHelper.Xstring2Byte(code);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ByteHelper.String2Byte(key);
            des.IV = ByteHelper.String2Byte(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return ByteHelper.Byte2String(ms.ToArray());
        }
    }
}
