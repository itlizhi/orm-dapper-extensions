using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dapper.Orm
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class DapperPagedList
    {
        //默认页码
        const int def = 20;
        //查询字段
        private static readonly Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //排序字段
        private static readonly Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //去重字段
        private static readonly Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql</param>
        /// <param name="param">查询参数</param>
        /// <param name="page">页面默认 1</param>
        /// <param name="pagesize">每页条数 默认10</param>
        /// <returns></returns>
        public static PagedList<T> Page<T>(this IDbConnection connection, string sql, object param = null, int page = 1, int pagesize = def)
        {
            string sqlPage;
            DynamicParameters pageParam;
            var p = Page<T>(connection, sql, param, page, pagesize, out sqlPage, out pageParam);
            p.Items = connection.Query<T>(sqlPage, pageParam);
            return p;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="page">当前页面</param>
        /// <param name="pagesize">每页的数量</param>
        /// <returns></returns>
        public static PagedList<dynamic> Page(this IDbConnection connection, string sql, dynamic param = null, int page = 1, int pagesize = def)
        {
            return connection.Page<dynamic>(sql, param as object, page, pagesize);
        }

        /// <summary>
        /// 分页查询，mapper
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static PagedList<TReturn> Page<TFirst, TSecond, TReturn>(this IDbConnection connection, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, int page = 1, int pagesize = def, string splitOn = "Id")
        {
            string sqlPage;
            DynamicParameters pageParam;
            var p = Page<TReturn>(connection, sql, param, page, pagesize, out sqlPage, out pageParam);
            p.Items = connection.Query(sqlPage, map, pageParam, splitOn: splitOn);
            return p;
        }

        /// <summary>
        /// 分页查询，mapper
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static PagedList<TReturn> Page<TFirst, TSecond, TThird, TReturn>(this IDbConnection connection, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, int page = 1, int pagesize = def, string splitOn = "Id")
        {
            string sqlPage;
            DynamicParameters pageParam;
            var p = Page<TReturn>(connection, sql, param, page, pagesize, out sqlPage, out pageParam);
            p.Items = connection.Query(sqlPage, map, pageParam, splitOn: splitOn);
            return p;
        }

        /// <summary>
        /// 构造基本Page
        /// </summary>
        /// <typeparam name="T">结构集</typeparam>
        /// <param name="sql">查询sql</param>
        /// <param name="param">查询参数</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">每页的条数</param>
        /// <param name="sqlPage">返回的sql</param>
        /// <param name="pageParam">返回的参数</param>
        /// <returns></returns>
        private static PagedList<T> Page<T>(IDbConnection connection, string sql, object param, int page, int pagesize, out string sqlPage, out DynamicParameters pageParam)
        {
            //替换 select filed  => select count(*)
            var m = rxColumns.Match(sql);
            // 获取 count(*)
            var g = m.Groups[1];

            //查询field
            var sqlSelectRemoved = sql.Substring(g.Index);

            var count = rxDistinct.IsMatch(sqlSelectRemoved) ? m.Groups[1].ToString().Trim() : "*";
            var sqlCount = string.Format("{0} count({1}) {2}", sql.Substring(0, g.Index), count, sql.Substring(g.Index + g.Length));
            //查找 order by filed
            m = rxOrderBy.Match(sqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }
            //查询总条数
            var total = connection.QueryFirstOrDefault<long>(sqlCount, param);

            //分页查询语句
            var connectionName = connection.GetType().Name.ToLower();
            string pagelimit; //分页关键字
            _sqlpage.TryGetValue(connectionName, out pagelimit);
            sqlPage = sql + pagelimit;

            pageParam = new DynamicParameters(param);
            pageParam.Add("@offset", (page - 1) * pagesize);
            pageParam.Add("@limit", pagesize);

            //总页码
            var totalPage = total / pagesize;
            if (total % pagesize != 0) totalPage++;

            var p = new PagedList<T>
            {
                PageSize = pagesize,
                CurrentPage = page,
                TotalPage = (int)totalPage,
                HasPrevious = page - 1 >= 1,
                HasNext = page + 1 <= totalPage,
                TotalItem = total
            };
            return p;
        }

        /// <summary>
        /// 分页的关键字，目前写了mysql 和 mssql
        /// </summary>
        private static readonly IDictionary<string, string> _sqlpage = new Dictionary<string, string>
                                                                                {
                                                                                    { "mysqlconnection", "\n limit @limit offset @offset" },
                                                                                    { "sqlconnection", "\n offset @offset row fetch next @limit rows only" }
                                                                                };

    }
}
