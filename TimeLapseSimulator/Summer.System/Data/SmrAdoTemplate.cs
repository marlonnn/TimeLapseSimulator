using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Spring.Data.Common;
using Spring.Data.Generic;
using Summer.System.Data.DbMapping;
using Summer.System.Data.VarietyDb;

namespace Summer.System.Data
{
    /// <summary>
    /// T类型的Ado方法类，该类为数据库核心，这是T类型的持久层
    /// <para>1. T类型须有public的空构造函数</para>
    /// <para>2. T类型和数据库关联的Property提供public的get_set方法</para>
    /// <para>3. T类型需和TableAttribute、FieldAttribute关联</para>
    /// </summary>
    /// <remarks>
    /// 公司：CASCO
    /// 作者： 戴唯艺
    /// 日期：2-13-5-23
    /// </remarks>
    public class SmrAdoTmplate<T> where T : new()
    {
        /// <summary>
        /// table名
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// property.Name和数据库Field定义的对应关系
        /// </summary>
        protected readonly Dictionary<string, FieldAttribute> fieldMapping;

        /// <summary>
        /// 数据持久化操作核心类
        /// </summary>
        protected AdoTemplate adoTmplte;
        /// <summary>
        /// 数据库sql转换器，参数配置工具
        /// </summary>
        protected ISqlConvertor convertor;
        /// <summary>
        /// 行数据转换器
        /// </summary>
        protected readonly Spring.Data.Generic.IRowMapper<T> rowMapper;
        public SmrAdoTmplate()
        {

            TableName = MappingHelper.GetTableName(typeof(T));
            fieldMapping = MappingHelper.Get_PropertyName_DbFieldDef(typeof(T));
			rowMapper = new RowMapper<T> ( );

        }
        /// <summary>
        /// dbKeyFieldList控制产生哪些数据库字段的parameter。为空时，将产生所有字段的parameter
        /// </summary>
        protected DbParameters CreateParams(T obj, List<string> dbKeyFieldList = null)
        {
            Dictionary<string, object> classValue = MappingHelper.GetPropertyValue(obj);
            var fieldMapping1 = new Dictionary<string, FieldAttribute>();
            if (dbKeyFieldList != null)
                foreach (var pair in fieldMapping)
                {
                    if (dbKeyFieldList.Contains(pair.Value.Name))
                        fieldMapping1[pair.Key] = pair.Value;
                }
            else
                fieldMapping1 = fieldMapping;

            return convertor.CreateParameters(fieldMapping1, classValue);


        }

        /// <summary>
        /// 插入数据，注意取值范围,不要插入自增标志
        /// </summary>
        /// <exception cref="DatabaseException">
        /// 如果数据超出范围，则抛出异常，特别是sql的datetime类型数据。查看InnerException定位具体数据库信息
        /// </exception>
        public void Insert(T obj)
        {

            List<string> fieldList = MappingHelper.GetDbField(fieldMapping);
            string sql = convertor.MakeInsertSql(TableName, fieldList);

            DbParameters params1 = CreateParams(obj);

            ExecuteNonQuery(sql, params1);
        }
        /// <summary>
        /// 根据obj中的PrimaryKey属性，更新。不要更新自增标识
        /// </summary>
        /// <exception cref="DatabaseException">
        /// 如果数据超出范围，则抛出异常，特别是sql的datetime类型数据。查看InnerException定位具体数据库信息
        /// </exception>
        public void Update(T obj)
        {
            List<string> dbMapping = MappingHelper.GetDbField(fieldMapping);
            List<string> dbKeyMapping = MappingHelper.GetKeyDbField(fieldMapping);

            //删除dbMapping中的Key属性字段，Key是不会被Update的
            dbMapping.RemoveAll(dbKeyMapping.Contains);

            string sql = convertor.MakeUpdateSql(TableName, dbMapping, dbKeyMapping);

            DbParameters param1 = CreateParams(obj);

            ExecuteNonQuery(sql, param1);
        }

        ///<summary>
        /// 需要注意，在Update、Insert时的异常问题
        /// </summary>
        /// <exception cref="DatabaseException">
        /// 如果数据超出范围，则抛出异常，特别是sql的datetime类型数据。查看InnerException定位具体数据库信息
        /// </exception>
        protected void ExecuteNonQuery(string sql, IDbParameters parameters)
        {
            try
            {
                adoTmplte.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception e)
            {
                //sql datetime范围:1753-01-01到9990-12-31；00：00：00到23:59:59.997 333毫秒
                //超出范围将报错
                //使用Sqlite数据库时，会存在数据库不能打开的情况，一般地，是因为数据库结构错误或版本不对造成
                //此情况请使用正确的工具重新生成数据库，建议：sqlite studio是能帮助您
                Exception inner = e.InnerException ?? e;
                var e1 = new DatabaseException(inner.Message, e.InnerException);
                throw e1;
            }
        }

        /// <summary>
        /// 根据obj中的PrimaryKey属性，删除
        /// </summary>
        public void Delete(T obj)
        {
            var dbKeyMapping = MappingHelper.GetKeyDbField(fieldMapping);
            var sql = convertor.MakeDeleteSql(TableName, dbKeyMapping);
            //param1只记录Key相关的数据
            DbParameters param1 = CreateParams(obj, dbKeyMapping);


            ExecuteNonQuery(sql, param1);
        }
        /// <summary>
        /// 根据obj中的PrimaryKey属性，查找
        /// </summary>
        /// <returns>没有对象，则返回Null</returns>
        public T Load(T obj)
        {
            List<string> dbKeyMapping = MappingHelper.GetKeyDbField(fieldMapping);
            string sql = convertor.MakeSelectSql(TableName, dbKeyMapping);

            DbParameters param1 = CreateParams(obj,dbKeyMapping);

            IList<T> list = adoTmplte.QueryWithRowMapper(CommandType.Text, sql, rowMapper, param1);
            return list.FirstOrDefault();
        }
        ///<summary>
        /// 如果该持久层提供的增删改查函数不能满足用户需求，可用此类
        /// <para>虽说是Find函数，但Sql语句可以任意增删改查</para>
        /// <para>用户需小心sql注入和各种未知异常问题（不推荐使用）</para>
        /// </summary>
        /// <returns>没有对象，则返回Null</returns>
        public T Find(string sql)
        {
            try
            {
                IList<T> list = adoTmplte.QueryWithRowMapper(CommandType.Text, sql, rowMapper);
                return list.FirstOrDefault();
            }
            catch (Exception e)
            {
                Exception inner = e.InnerException ?? e;
                var e1 = new DatabaseException(inner.Message, e.InnerException);
                throw e1;
            }
        }

        public IList<T> FindAll(string sql)
        {
            try
            {
                IList<T> list = adoTmplte.QueryWithRowMapper(CommandType.Text, sql, rowMapper);
                return list;
            }
            catch (Exception e)
            {
                Exception inner = e.InnerException ?? e;
                var e1 = new DatabaseException(inner.Message, e.InnerException);
                throw e1;
            }

        }
    }
}