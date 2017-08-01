using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;

namespace DapperExtensions.Extend
{
    public class DapperContext : IDapperContext
    {
        string _dbType { get; set; }
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

        public string DBType
        {
            get { return _dbType; }
        }

        public DapperContext(string appConfigKey, string dbType = DbType.MySql)
        {
            _db = SqlFactory.GetDbInstance(appConfigKey, dbType);
            _dbType = dbType;
            _db.Open();
            _builder = new SqlBuilder(SqlFactory.GetDapperConfiguration(dbType));
        }
        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键</param>
        /// <param name="value">主键值</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T Get<T, T2>(string id, T2 value, int? commandTimeout = default(int?))
            where T : class
            where T2 : struct
        {
            using (DB)
            {
                var sql = _builder.SelectSingle<T>(id);
                DynamicParameters param = new DynamicParameters();
                param.Add(id, value);
                return DB.Query<T>(sql, param).SingleOrDefault();
            }
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Insert<T>(IEnumerable<T> entities, string primaryKey, int? commandTimeout = default(int?)) where T : class
        {
            DB.Open();
            var transaction = DB.BeginTransaction();
            var result = 0;
            try
            {
                var sql = _builder.Insert(entities.First(), primaryKey);
                result = DB.Execute(sql, entities, transaction, commandTimeout);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                DB.Close();
            }
            return result;
        }
        /// <summary>
        /// 添加返回影响条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="primaryKey"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Insert<T>(T entity, string primaryKey, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                var sql = _builder.Insert(entity, primaryKey);
                return DB.Execute(sql, entity);
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
        public bool Update<T>(IPredicate predicate, IDictionary<string, object> set, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                IDictionary<string, object> where = new Dictionary<string, object>();
                var sql = _builder.Update<T>(predicate, where, set.Select(x => x.Key));
                DynamicParameters param = new DynamicParameters();
                where.AsList().ForEach(x => param.Add(x.Key, x.Value));
                set.AsList().ForEach(x => param.Add(x.Key, x.Value));
                return DB.Execute(sql, param) > 0 ? true : false;
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
        public int Count<T>(IPredicate predicate, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.Count<T>(predicate);
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
        public IEnumerable<T> GetPage<T>(IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                if (page < 1)
                {
                    page = 1;
                }
                page -= 1;
                return DB.GetPage<T>(predicate, sort, page, resultsPerPage);
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
        public IEnumerable<T> GetList<T>(IPredicate predicate, IList<ISort> sort, int? commandTimeout = default(int?)) where T : class
        {
            using (DB)
            {
                return DB.GetList<T>(predicate, sort);
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
