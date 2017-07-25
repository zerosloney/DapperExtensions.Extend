using System;
using System.Linq.Expressions;

namespace DapperExtensions.Extend
{

    /// <summary>
    /// 数据字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbFiled<T>
    {
        /// <summary>
        /// 实体表达式字段
        /// </summary>
        public Expression<Func<T, object>> Name { get; private set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; private set; }

        public DbFiled(Expression<Func<T, object>> name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// 字段排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Sorting<T> where T : class
    {
        /// <summary>
        /// 排序字段表达式
        /// </summary>
        public Expression<Func<T, object>> Parameter { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType Direction { get; set; }
    }

}
