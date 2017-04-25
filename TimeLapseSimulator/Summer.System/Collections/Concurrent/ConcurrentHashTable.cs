using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Summer.System.Collections.Concurrent
{
    /// <summary>
    /// 安全HashTable
    /// </summary>
    /// <remarks>
    /// 思路：封装线程安全的hashTable
    /// 公司：CASCO
    /// 作者：袁松
    /// 日期：2013-05-21
    /// 说明：主要包含增加、删除、修改、读取等方法
    /// </remarks>
    public class ConcurrentHashTable
    {
        private Hashtable hashTable;

        /// <summary>
        /// 构造函数1，构造线程安全的空hashTable
        /// </summary>
        public ConcurrentHashTable()
        {
            hashTable = Hashtable.Synchronized(new Hashtable());
        }

        /// <summary>
        /// 构造函数2，从字典构造hashTable
        /// </summary>
        /// <param name="dic"></param>
        public ConcurrentHashTable(IDictionary dic)
        {
            hashTable = Hashtable.Synchronized(new Hashtable(dic));
        }
        /// <summary>
        /// 增加一条记录至HashTable，只有当key不存在时才添加，如果已存在key，则不添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AppendData(object key,object value)
        {
            if (!hashTable.Contains(key))
            {
                hashTable.Add(key, value);
            }
        }

        /// <summary>
        /// 修改hashTable中的某一条记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void UpdateData(object key,object value)
        {
            hashTable[key] = value;
        }

        /// <summary>
        /// 删除hashTable中的某一条记录
        /// </summary>
        /// <param name="key">键</param>
        public void RemoveData(object key)
        {
            hashTable.Remove(key);
        }

        /// <summary>
        /// 清空hashTable
        /// </summary>
        public void ClearData()
        {
            hashTable.Clear();
        }

        /// <summary>
        /// 根据Key获取对应的Value
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public object GetValue(object key)
        {
            if(hashTable.ContainsKey(key))
            {
                return hashTable[key];
            }
            return null;
        }

        /// <summary>
        /// 获取hashTable中键值对的个数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return hashTable.Count;
        }
    }
}
