using System.Collections.Generic;
using System.Data;

namespace DapperExtensions.Extend
{
    public interface IDapperContext
    {
        /// <summary>
        /// DbConnection
        /// </summary>
        IDbConnection DB { get; }
        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TReturn> SqlQuery<TReturn>(string sql, object param) where TReturn : class;
        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键Id</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        T Get<T>(int id, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 添加一组实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Insert<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 更新语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">where条件</param>
        /// <param name="set">更新字段</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        bool Update<T>(IPredicate predicate, IDictionary<string, object> set, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 实体的数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">where条件</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Count<T>(IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 获取实体集合(分页)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">where条件</param>
        /// <param name="sort">排序</param>
        /// <param name="page">页</param>
        /// <param name="resultsPerPage">每页显示条数</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> GetPage<T>(IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(IPredicate predicate, IList<ISort> sort, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class;
    }
}
