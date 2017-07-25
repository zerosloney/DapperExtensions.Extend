using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DapperExtensions.Extend
{
    public interface IRespositoryBase<T> where T:class
    {
        /// <summary>
        /// 数据上下文实例
        /// </summary>
        IDapperContext Context { get; }

        #region 同步方法
        /// <summary>
        /// 根据主键Id获取一个实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        T Get(int id);
        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Insert(T entity);
        /// <summary>
        /// 批量添加一个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?));
        /// <summary>
        /// 根据表达式查询实体的数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 根据表达式查询获取实体分页集合
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="sorts">排序</param>
        /// <param name="page">页数</param>
        /// <param name="resultsPerPage">每页显示数量</param>
        /// <param name="isTotal">是否查询实体数量</param>
        /// <param name="total">返回实体数量</param>
        /// <returns></returns>
        IEnumerable<T> GetPage(Expression<Func<T, bool>> expression, Sorting<T>[] sorts, int page, int resultsPerPage, bool isTotal, ref int total);
        /// <summary>
        /// 根据表达式查询获取实体集合
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="sorts">排序</param>
        /// <returns></returns>
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression, Sorting<T>[] sorts);
        /// <summary>
        /// 根据表达式更新实体
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="fileds">更新字段</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, bool>> expression, params DbFiled<T>[] fileds); 
        #endregion
    }
}
