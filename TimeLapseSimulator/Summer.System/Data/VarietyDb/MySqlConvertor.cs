using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.Common;
using Summer.System.Data.DbMapping;
using System.Data;
using MySql.Data.MySqlClient;

namespace Summer.System.Data.VarietyDb
{
    public class MySqlConvertor : ISqlConvertor
    {
        /// <summary>
        /// 必须为protect方式,如此继承，则可通过spring配置，private的字段由于在继承类不可见，故不能被spring察觉到
        /// </summary>
        protected IDbProvider provider;

        /// <summary>
        /// 一般地，只需要重写此函数，修改Parameter的类型即可
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual IDataParameter CreateDataParameter(string name, object value)
        {
            return new MySqlParameter(name, value);
        }

        /// <summary>
        /// 重写此函数，可以产生对应不同数据库的参数类型
        /// <para>以dbFieldDef为基准</para>
        /// </summary>
        /// <returns></returns>
        public virtual DbParameters CreateParameters(Dictionary<string, FieldAttribute> dbFieldDef,
            Dictionary<string, object> classValue)
        {
            var dbMapping = new DbParameters(provider);
            foreach (var map1 in dbFieldDef)
            {

                string dbFieldName = map1.Value.Name;
                //Sql语句不接受null类型的参数，此情况需转换为空串
                object value = classValue[map1.Key] ?? string.Empty;

                //只要为IDataParameter的子类就能加入
                //包括OdbcParameter，OracleParameter，OleDbParameter，SqlParameter等等
                //在此新建不同的Parameter，就能对应不同的数据库
                IDataParameter pa = CreateDataParameter(dbFieldName, value);

                dbMapping.AddParameter(pa);
            }
            return dbMapping;
        }

        /// <summary>
        /// 重写此函数，可以实现针对sql字串的不同参数配置语法
        /// </summary>
        /// <returns></returns>
        public virtual string paramCreator(string field)
        {
            return provider.DbMetadata.ParameterNamePrefix + field;
        }

        /// <summary>
        /// 在Update，Select，Delete语句中需要此函数设置Where属性
        /// <para>此函数的where语句仅包括and方法</para>
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected virtual string whereCreator(List<string> fields)
        {
            string wheres = "";
            foreach (var field in fields)
            {
                wheres += field + "=" + paramCreator(field) + " and ";
            }
            if (wheres.Length > 0)
                wheres = wheres.Remove(wheres.Length - 4, 4);
            return wheres;
        }

        public string MakeDeleteSql(string tableName, List<string> dbKeyFieldList)
        {
            string wheres = whereCreator(dbKeyFieldList);
            return string.Format("delete from {0} where {1}", tableName, wheres);
        }

        public string MakeInsertSql(string tableName, List<string> dbFieldList)
        {
            var fieldStr = "";
            var values = "";
            foreach (var field in dbFieldList)
            {
                fieldStr += field + ",";
                values += paramCreator(field) + ",";
            }
            if (fieldStr.Length > 0)//去掉最后逗号
            {
                fieldStr = fieldStr.Remove(fieldStr.Length - 1);
                values = values.Remove(values.Length - 1);
            }

            return string.Format("insert into {0}({1}) values({2})", tableName, fieldStr, values);
        }

        public string MakeSelectSql(string tableName, List<string> dbKeyFieldList)
        {
            string wheres = whereCreator(dbKeyFieldList);
            return string.Format("select * from {0} where {1}", tableName, wheres);
        }

        public string MakeUpdateSql(string tableName, List<string> dbFieldListWithoutKey, List<string> dbKeyFieldList)
        {
            string sets = "";
            string wheres = "";
            foreach (var field in dbFieldListWithoutKey)
            {
                sets += field + "=" + paramCreator(field) + ",";
            }
            if (sets.Length > 0)
                sets = sets.Remove(sets.Length - 1);

            wheres = whereCreator(dbKeyFieldList);

            return string.Format("update {0} set {1} where {2}", tableName, sets, wheres);
        }
    }
}
