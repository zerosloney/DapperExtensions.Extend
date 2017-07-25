using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;

namespace DapperExtensions.Extend
{
    public class DapperContext : IDapperContext
    {
        /// <summary>
        /// dapper数据库连接实例
        /// </summary>
        IDbConnection _db;
        /// <summary>
        /// sql生成
        /// </summary>
        SqlBuilder _builder;
        /// <summary>
        /// dapper数据库连接实例
        /// </summary>
        public IDbConnection DB
        {
            get
            {
                return _db;
            }
        }
        public DapperContext(string appConfigKey, string dbType = DbType.MySql)
        {
            _db = SqlFactory.GetDbInstance(appConfigKey, dbType);
            _builder = new SqlBuilder(SqlFactory.GetDapperConfiguration(dbType));
        }
        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T Get<T>(int id, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.Get<T>(id, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        public void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                DB.Insert(entities, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Insert<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.Insert(entity, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="set"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool Update<T>(IPredicate predicate, IDictionary<string, object> set, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                IDictionary<string, object> where = new Dictionary<string, object>();
                var sql = _builder.Update<T>(predicate, where, set.Select(x => x.Key));
                DynamicParameters param = new DynamicParameters();
                where.AsList().ForEach(x => param.Add(x.Key, x.Value));
                set.AsList().ForEach(x => param.Add(x.Key, x.Value));
                return DB.Execute(sql, param, transaction, commandTimeout) > 0 ? true : false;
            }
        }
        /// <summary>
        /// 实体数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Count<T>(IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.Count<T>(predicate, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// 获取实体集合(分页)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPage<T>(IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                if (page < 1)
                {
                    page = 1;
                }
                page -= 1;
                return DB.GetPage<T>(predicate, sort, page, resultsPerPage, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(IPredicate predicate, IList<ISort> sort, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.GetList<T>(predicate, sort, transaction, commandTimeout);
            }
        }
        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> SqlQuery<TReturn>(string sql, object param) where TReturn : class
        {
            using (DB)
            {
                return DB.Query<TReturn>(sql, param);
            }
        }
    }
}
