using System.Collections.Generic;
using Castle.DynamicProxy;

namespace DapperExtensions.Extend
{
    internal class SqlLogInterceptor : IInterceptor
    {
        SqlBuilder _builder;
        public SqlLogInterceptor(string appConfigKey, string dbType = DbType.MySql)
        {
            _builder = new SqlBuilder(SqlFactory.GetDapperConfiguration(dbType));
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            var args = invocation.Arguments;
            var entityType = invocation.GenericArguments[0];
            var sql = string.Empty;
            switch (method)
            {
                case "GetPage":
                    sql = _builder.GetPagingWithMySql(entityType, args[0] as IPredicate, args[1] as IList<ISort>, int.Parse(args[2].ToString()), int.Parse(args[3].ToString()));
                    break;
            }
            WriteSqlToConsole(sql);
            invocation.Proceed();
        }

        private void WriteSqlToConsole(string sql)
        {
            System.Diagnostics.Debug.Write(sql);
        }
    }
}
