using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperExtensions.Extend
{
    internal static class SortingEx
    {
        /// <summary>
        /// 排序组转成DapperExtensions的IList<ISort>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static IList<ISort> ToSortable<T>(this IEnumerable<Sorting<T>> sorts) where T : class
        {
            var sortList = new List<ISort>();
            if (sorts == null || sorts.Count() == 0)
            {
                return sortList;
            }
            foreach (var sort in sorts)
            {
                MemberInfo sortProperty = ReflectionHelper.GetProperty(sort.Parameter);
                sortList.Add(new Sort
                {
                    Ascending = sort.Direction != SortType.Desc,
                    PropertyName = sortProperty.Name
                });
            }
            return sortList;
        }
    }

}
