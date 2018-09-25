using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbOperationByDapper
{
    public interface ISQLHelper<T>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> Insert(T t);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> Update(T t);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> Delete(T t);
    }
}
