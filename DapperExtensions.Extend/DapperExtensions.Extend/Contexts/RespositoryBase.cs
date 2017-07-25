using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DapperExtensions.Extend
{
    public class RespositoryBase<T> : IRespositoryBase<T> where T : class
    {
        public IDapperContext Context { get { return _context; } }

        IDapperContext _context;

        public RespositoryBase(IDapperContext context)
        {
            _context = context;
        }

        #region 同步方法

        /// <summary>
        /// 根据表达式查询实体的数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> expression)
        {
            return _context.Count<T>(expression.ToPredicateGroup());
        }
        /// <summary>
        /// 根据主键Id获取一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id)
        {
            return _context.Get<T>(id);
        }
        /// <summary>
        /// 实体集合
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(Expression<Func<T, bool>> expression, Sorting<T>[] sorts)
        {
            return _context.GetList<T>(expression.ToPredicateGroup(), sorts.ToSortable());
        }
        /// <summary>
        /// 分页集合
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sorts"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <param name="isTotal"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPage(Expression<Func<T, bool>> expression, Sorting<T>[] sorts, int page, int resultsPerPage, bool isTotal, ref int total)
        {
            var predicates = expression.ToPredicateGroup();
            if (isTotal)
            {
                total = _context.Count<T>(predicates);
            }
            return _context.GetPage<T>(predicates, sorts.ToSortable(), page, resultsPerPage);
        }
        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(T entity)
        {
            return _context.Insert(entity) > 0 ? true : false;
        }
        /// <summary>
        /// 批量添加一个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?))
        {
            try
            {
                _context.Insert(entities, transaction, commandTimeout);
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, bool>> expression, params DbFiled<T>[] fileds)
        {
            return _context.Update<T>(expression.ToPredicateGroup(), fileds.ToPropertyParam());
        } 
        #endregion
    }
}
