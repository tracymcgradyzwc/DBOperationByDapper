using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DbOperationByDapper.Cache;
using System.ComponentModel;
using DbOperationByDapper.AttributeHelper;

namespace DbOperationByDapper.PostgreSQL
{
    public class PostgreSQLHelper<T> : IPostgreSQLHelper<T>
    {
        private static string ConnectionString = string.Empty;
        public PostgreSQLHelper(string strConnection)
        {
            ConnectionString = strConnection;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Insert(T t)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var insertSQL = InsertStr(t);
                    int returnValue = conn.Execute(insertSQL, t);
                    conn.Close();
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改(条件只支持AND)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Update(T t)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                string updateSQL = UpdateStr(t);
                int returnValue = conn.Execute(updateSQL, t);
                conn.Close();
                return returnValue;
            }
        }
        /// <summary>
        /// 删除(条件只支持AND)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Delete(T t)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                string deleteSQL = DeleteStr(t);
                int returnValue = conn.Execute(deleteSQL, t);
                conn.Close();
                return returnValue;
            }
        }


        /// <summary>
        /// 新增字符串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string InsertStr(T t)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder keyStr = new StringBuilder();
            StringBuilder valueStr = new StringBuilder();
            Type type = t.GetType();
            string cacheName = "Insert_" + type.Name;
            var sqlStr = CacheHelper.Get<string>(cacheName);
            if (null != sqlStr)
            {
                return sqlStr;
            }
            if (t == null)
            {
                return stringBuilder.ToString();
            }
            //获取所有属性
            System.Reflection.PropertyInfo[] properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return stringBuilder.ToString();
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (null != value)
                    {
                        keyStr = keyStr.AppendFormat("\"{0}\",", name);
                        valueStr = valueStr.AppendFormat("@{0},", name);
                    }
                }
            }
            stringBuilder = stringBuilder.AppendFormat("insert into public.\"{0}\" ({1}) values({2})", type.Name, keyStr.Remove(keyStr.Length - 1, 1), valueStr.Remove(valueStr.Length - 1, 1));
            CacheHelper.Set(cacheName, stringBuilder.ToString());
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 修改字符串（条件只支持and）
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string UpdateStr(T t)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder keyValueStr = new StringBuilder();
            StringBuilder valueStr = new StringBuilder();
            StringBuilder whereStr = new StringBuilder();
            Type type = t.GetType();
            string cacheName = "Update_" + type.Name;
            var sqlStr = CacheHelper.Get<string>(cacheName);
            if (null != sqlStr)
            {
                return sqlStr;
            }
            if (t == null)
            {
                return stringBuilder.ToString();
            }
            //获取所有属性
            System.Reflection.PropertyInfo[] properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return stringBuilder.ToString();
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (null != value)
                    {
                        var contain = item.GetCustomAttributesData().FirstOrDefault(s => s.AttributeType == typeof(WhereAttribute));
                        if (null != contain)
                        {
                            if (contain.ConstructorArguments.Count > 0 && null != contain.ConstructorArguments[0])
                                whereStr.AppendFormat(" and \"{0}\" {1} @{0}", name, contain.ConstructorArguments[0].Value);
                            else
                                whereStr.AppendFormat(" and \"{0}\"=@{0}", name);
                        }
                        keyValueStr.AppendFormat("\"{0}\"=@{0},", name);
                    }
                }
            }
            stringBuilder = stringBuilder.AppendFormat("Update public.\"{0}\" set {1} where 1=1 {2}", type.Name, keyValueStr.Remove(keyValueStr.Length - 1, 1), whereStr);
            CacheHelper.Set(cacheName, stringBuilder.ToString());
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 删除（套件只支持and）
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string DeleteStr(T t)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder whereStr = new StringBuilder();
            Type type = t.GetType();
            string cacheName = "Delete_" + type.Name;
            var sqlStr = CacheHelper.Get<string>(cacheName);
            if (null != sqlStr)
            {
                return sqlStr;
            }
            if (t == null)
            {
                return stringBuilder.ToString();
            }

            //获取所有属性
            System.Reflection.PropertyInfo[] properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return stringBuilder.ToString();
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (null != value)
                    {
                        var contain = item.GetCustomAttributesData().FirstOrDefault(s => s.AttributeType == typeof(WhereAttribute));
                        if (null != contain)
                        {
                            if (contain.ConstructorArguments.Count > 0 && null != contain.ConstructorArguments[0])
                                whereStr.AppendFormat(" and \"{0}\" {1} @{0}", name, contain.ConstructorArguments[0].Value);
                            else
                                whereStr.AppendFormat(" and \"{0}\"=@{0}", name);
                        }
                    }
                }
            }
            stringBuilder = stringBuilder.AppendFormat("Delete from public.\"{0}\" where 1=1 {1}", type.Name, whereStr);
            CacheHelper.Set(cacheName, stringBuilder.ToString());
            return stringBuilder.ToString();
        }
    }

}
