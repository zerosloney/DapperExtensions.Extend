using System.Data;
using MySql.Data.MySqlClient;
using DapperExtensions.Sql;
using DapperExtensions.Mapper;
using System.Reflection;
using System.Collections.Generic;

namespace DapperExtensions.Extend
{
    internal class SqlFactory
    {
        /// <summary>
        /// dapper连接实例
        /// </summary>
        /// <param name="confKey">webconfig配置键</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static IDbConnection GetDbInstance(string confKey, string dbType = DbType.MySql)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[confKey].ConnectionString;
            if (dbType == DbType.MySql)
            {
                DapperExtensions.SqlDialect = new MySqlDialect();
                return new MySqlConnection(connectionString);
            }
            return null;
        }

        /// <summary>
        /// SqlGenerator实例
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ISqlGenerator GetSqlGenerator(IDapperExtensionsConfiguration configuration)
        {
            return new SqlGeneratorImpl(configuration);
        }

        /// <summary>
        /// DapperExtensions配置实例
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static IDapperExtensionsConfiguration GetDapperConfiguration(string dbType = DbType.MySql)
        {
            if (dbType == DbType.MySql)
            {
                return new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            }
            return null;
        }



    }
}
