using DbOperationByDapper.Enum;
using DbOperationByDapper.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbOperationByDapper
{
    public class DbOperationByDapper<T>
    {
        public static ISQLHelper<T> sQLHelper;
        /// <summary>
        /// DbType（0:PostGre,1:SqlServer,2:MySql）
        /// </summary>
        /// <param name="DbType"></param>
        public DbOperationByDapper(int DbType, string strConnection)
        {
            if ((int)DbTypeEnum.Postgre == DbType)
                sQLHelper = new PostgreSQLHelper<T>(strConnection);
            //if ((int)DbTypeEnum.SqlServer == DbType)
            //    sQLHelper = new PostgreSQLHelper<T>(strConnection);
            //if ((int)DbTypeEnum.MySQL == DbType)
            //    sQLHelper = new PostgreSQLHelper<T>(strConnection);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Insert(T t)
        {
            return await sQLHelper.Insert(t);
        }
        /// <summary>
        /// 修改（条件只支持and）
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Update(T t)
        {
            return await sQLHelper.Update(t);
        }
        /// <summary>
        /// 删除（条件只支持and）
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> Delete(T t)
        {
            return await sQLHelper.Delete(t);
        }
    }
}
